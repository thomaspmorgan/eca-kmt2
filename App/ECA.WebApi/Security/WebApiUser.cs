using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using System.Security.Claims;
using System.Threading;
using System.Security.Principal;
using System.Linq.Expressions;
using ECA.Core.Logging;
using System.Diagnostics.Contracts;

namespace ECA.WebApi.Security
{

    public class ResourcePermission
    {
        public int ResourceId { get; set; }

        public string ResourceType { get; set; }

        public string PermissionName { get; set; }

        /// <summary>
        /// Returns true if the given object equals this object.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>True if the given object equals this object.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var otherType = obj as ResourcePermission;
            if (otherType == null)
            {
                return false;
            }
            return this.ResourceId == otherType.ResourceId
                && this.ResourceType == otherType.ResourceType
                && this.PermissionName == otherType.PermissionName;

        }

        public override int GetHashCode()
        {
            var hash = ResourceId * 27;
            hash += ResourceType.GetHashCode() * 27;
            hash += PermissionName.GetHashCode();
            return hash;
        }

    }

    public interface IWebApiUser
    {
        Guid Id { get; }
    }

    public abstract class WebApiUserBase : IWebApiUser
    {
        public abstract string GetUsername();

        public abstract ECA.Business.Service.User ToBusinessUser();

        public abstract bool HasPermission(ResourcePermission requestedPermission, IEnumerable<ResourcePermission> allUserPermissions);

        public Guid Id
        {
            get;
            protected set;
        }
    }



    public class WebApiUser : WebApiUserBase
    {
        public const string ISSUED_AT_TIME_KEY = "iat";

        public const string VALID_NOT_BEFORE_DATE_KEY = "nbf";

        public const string EXPIRATION_DATE_KEY = "exp";

        public const string USER_ID_KEY = "http://schemas.microsoft.com/identity/claims/objectidentifier";

        public const string EMAIL_ID_KEY = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

        public const string GIVEN_NAME_KEY = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";

        public const string SURNAME_KEY = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";

        public const string FULL_NAME_KEY = "name";

        private const string MISSING_CLAIM_ERROR_MESSAGE = "The bearer token does not contain a claim with the key [{0}].";

        private const string UNABLE_TO_PARSE_SECONDS_ERROR_MESSAGE = "Unable to parse seconds as an integer from the claim value [{0}] with claim type [{1}].";

        public static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly ILogger logger;
        private readonly IEnumerable<Claim> claims;

        internal WebApiUser(ILogger logger, IEnumerable<Claim> claims)
        {
            Contract.Requires(logger != null, "The logger must not be null.");
            this.logger = logger;
            this.claims = claims;
            SetFullName(claims);
            SetGivenName(claims);
            SetSurname(claims);
            SetTokenExpirationDate(claims);
            SetTokenIssueDate(claims);
            SetTokenValidAfterDate(claims);
            SetUserEmail(claims);
            SetUserId(claims);
        }

        internal WebApiUser(ILogger logger, ClaimsPrincipal principal)
            : this(logger, principal.Claims)
        {
            Contract.Requires(logger != null, "The logger must not be null.");
            Contract.Requires(principal != null, "The principal must not be null.");
        }

        public WebApiUser(ILogger logger, IPrincipal principal)
            : this(logger, (principal as ClaimsPrincipal))
        {
            Contract.Requires(logger != null, "The logger must not be null.");
            Contract.Requires(principal is ClaimsPrincipal, "The IPrincipal instance must be a ClaimsPrincipal.");
        }

        public string Email { get; protected set; }
        public string GivenName { get; protected set; }
        public string SurName { get; protected set; }
        public string FullName { get; protected set; }
        public DateTime TokenExpiration { get; protected set; }
        public DateTime TokenIssuedDate { get; protected set; }
        public DateTime TokenValidAfterDate { get; protected set; }

        public IEnumerable<Claim> GetClaims()
        {
            return this.claims;
        }

        private Claim GetClaimByType(IEnumerable<Claim> claims, string type)
        {
            return claims.ToList().Where(c => c.Type == type).FirstOrDefault();
        }

        private void LogMissingClaimError(string claimType)
        {
            this.logger.Error(MISSING_CLAIM_ERROR_MESSAGE, claimType);
        }

        private void LogMissingClaimWarning(string claimType)
        {
            this.logger.Warning(MISSING_CLAIM_ERROR_MESSAGE, claimType);
        }

        public void SetUserId(IEnumerable<Claim> claims)
        {
            Contract.Requires(claims != null, "The claims must not be null.");
            var key = USER_ID_KEY;
            var claim = GetClaimByType(claims, key);
            if (claim != null)
            {
                Guid id;
                if (Guid.TryParse(claim.Value, out id))
                {
                    this.Id = id;
                }
                else
                {
                    this.logger.Error("Unable to parse user id, with value [{0}] from token.", claim.Value);
                }
            }
            else
            {
                LogMissingClaimError(key);
            }
        }

        public void SetUserEmail(IEnumerable<Claim> claims)
        {
            Contract.Requires(claims != null, "The claims must not be null.");
            var key = EMAIL_ID_KEY;
            var claim = GetClaimByType(claims, key);
            if (claim != null)
            {
                this.Email = claim.Value;
            }
            else
            {
                LogMissingClaimWarning(key);
            }
        }

        public void SetGivenName(IEnumerable<Claim> claims)
        {
            Contract.Requires(claims != null, "The claims must not be null.");
            var key = GIVEN_NAME_KEY;
            var claim = GetClaimByType(claims, key);
            if (claim != null)
            {
                this.GivenName = claim.Value;
            }
            else
            {
                LogMissingClaimWarning(key);
            }
        }

        public void SetSurname(IEnumerable<Claim> claims)
        {
            Contract.Requires(claims != null, "The claims must not be null.");
            var key = SURNAME_KEY;
            var claim = GetClaimByType(claims, key);
            if (claim != null)
            {
                this.SurName = claim.Value;
            }
            else
            {
                LogMissingClaimWarning(key);
            }
        }

        public void SetFullName(IEnumerable<Claim> claims)
        {
            Contract.Requires(claims != null, "The claims must not be null.");
            var key = FULL_NAME_KEY;
            var claim = GetClaimByType(claims, key);
            if (claim != null)
            {
                this.FullName = claim.Value;
            }
            else
            {
                LogMissingClaimWarning(key);
            }
        }

        public void SetTokenIssueDate(IEnumerable<Claim> claims)
        {
            Contract.Requires(claims != null, "The claims must not be null.");
            var key = ISSUED_AT_TIME_KEY;
            var claim = GetClaimByType(claims, key);
            if (claim != null)
            {
                int secondsAfterEpoc;
                if (Int32.TryParse(claim.Value, out secondsAfterEpoc))
                {
                    this.TokenIssuedDate = GetUTCDate(secondsAfterEpoc);
                }
                else
                {
                    logger.Warning(UNABLE_TO_PARSE_SECONDS_ERROR_MESSAGE, claim.Value, key);
                }
            }
            else
            {
                LogMissingClaimWarning(key);
            }
        }

        public void SetTokenExpirationDate(IEnumerable<Claim> claims)
        {
            Contract.Requires(claims != null, "The claims must not be null.");
            var key = EXPIRATION_DATE_KEY;
            var claim = GetClaimByType(claims, key);
            if (claim != null)
            {
                int secondsAfterEpoc;
                if (Int32.TryParse(claim.Value, out secondsAfterEpoc))
                {
                    this.TokenExpiration = GetUTCDate(secondsAfterEpoc);
                }
                else
                {
                    logger.Warning(UNABLE_TO_PARSE_SECONDS_ERROR_MESSAGE, claim.Value, key);
                }
            }
            else
            {
                LogMissingClaimWarning(key);
            }
        }

        public void SetTokenValidAfterDate(IEnumerable<Claim> claims)
        {
            Contract.Requires(claims != null, "The claims must not be null.");
            var key = VALID_NOT_BEFORE_DATE_KEY;
            var claim = GetClaimByType(claims, key);
            if (claim != null)
            {
                int secondsAfterEpoc;
                if (Int32.TryParse(claim.Value, out secondsAfterEpoc))
                {
                    this.TokenValidAfterDate = GetUTCDate(secondsAfterEpoc);
                }
                else
                {
                    logger.Warning(UNABLE_TO_PARSE_SECONDS_ERROR_MESSAGE, claim.Value, key);
                }
            }
            else
            {
                LogMissingClaimWarning(key);
            }
        }

        public override ECA.Business.Service.User ToBusinessUser()
        {
            return new Business.Service.User(-1);
        }

        public override bool HasPermission(ResourcePermission requestedPermission, IEnumerable<ResourcePermission> allUserPermissions)
        {
            return true;
        }

        public DateTime GetUTCDate(int secondsAfterEpoch)
        {
            return EPOCH.AddSeconds((double)secondsAfterEpoch);
        }

        public override string GetUsername()
        {
            return this.Email;
        }
    }
}