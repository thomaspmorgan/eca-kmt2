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
using CAM.Business.Service;

namespace ECA.WebApi.Security
{
    public interface IWebApiUser
    {
        Guid Id { get; }

        string GetUsername();
    }

    public abstract class WebApiUserBase : IWebApiUser
    {
        public abstract bool HasPermission(IPermission requestedPermission, IEnumerable<IPermission> allUserPermissions);

        public Guid Id
        {
            get;
            protected set;
        }

        public abstract string GetUsername();
    }


    /// <summary>
    /// The WebApiUser is a user that user that has been validated by Microsoft Azure.
    /// </summary>
    public class WebApiUser : WebApiUserBase
    {
        /// <summary>
        /// Azure's token issued at date key.
        /// </summary>
        public const string ISSUED_AT_TIME_KEY = "iat";

        /// <summary>
        /// Azure's token valid not before date key.  
        /// </summary>
        public const string VALID_NOT_BEFORE_DATE_KEY = "nbf";

        /// <summary>
        /// Azure's token expiration date key.
        /// </summary>
        public const string EXPIRATION_DATE_KEY = "exp";

        /// <summary>
        /// Azure's user id token key.
        /// </summary>
        public const string USER_ID_KEY = "http://schemas.microsoft.com/identity/claims/objectidentifier";

        /// <summary>
        /// Azure's email id token key.
        /// </summary>
        public const string EMAIL_KEY = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        /// <summary>
        /// Azure's given name token key.
        /// </summary>
        public const string GIVEN_NAME_KEY = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";

        /// <summary>
        /// Azure's surname token key.
        /// </summary>
        public const string SURNAME_KEY = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";

        /// <summary>
        /// Azure's full name key.
        /// </summary>
        public const string FULL_NAME_KEY = "name";


        private const string MISSING_CLAIM_ERROR_MESSAGE = "The bearer token does not contain a claim with the key [{0}].";

        private const string UNABLE_TO_PARSE_SECONDS_ERROR_MESSAGE = "Unable to parse seconds as an integer from the claim value [{0}] with claim type [{1}].";

        public static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly ILogger logger;
        private readonly IEnumerable<Claim> claims;

        /// <summary>
        /// Initializes this user with the given logger and claims.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="claims">The user's claims.</param>
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

        /// <summary>
        /// Initializes a new user with the given logger and IPrincipal.  Currently, the IPrincipal must
        /// be a ClaimsPrincipal.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="principal">The claims principal as an IPrincipal instance.</param>
        public WebApiUser(ILogger logger, IPrincipal principal)
            : this(logger, (principal as ClaimsPrincipal))
        {
            Contract.Requires(logger != null, "The logger must not be null.");
            Contract.Requires(principal is ClaimsPrincipal, "The IPrincipal instance must be a ClaimsPrincipal.");
        }

        /// <summary>
        /// Gets the user's email.
        /// </summary>
        public string Email { get; protected set; }

        /// <summary>
        /// Gets the user's given name.
        /// </summary>
        public string GivenName { get; protected set; }

        /// <summary>
        /// Gets the user's surname.
        /// </summary>
        public string SurName { get; protected set; }

        /// <summary>
        /// Gets the user's full name.
        /// </summary>
        public string FullName { get; protected set; }

        /// <summary>
        /// Gets this user's token expiration date.
        /// </summary>
        public DateTime TokenExpirationDate { get; protected set; }

        /// <summary>
        /// Gets this user's token issued date.
        /// </summary>
        public DateTime TokenIssuedDate { get; protected set; }

        /// <summary>
        /// Gets this user's earliest date the token is valid.
        /// </summary>
        public DateTime TokenValidAfterDate { get; protected set; }

        /// <summary>
        /// Returns the claims of this user.
        /// </summary>
        /// <returns>The user's claims.</returns>
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

        /// <summary>
        /// Sets the user id given the claims.
        /// </summary>
        /// <param name="claims">The user claims.</param>
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

        /// <summary>
        /// Sets the user's email given the claims.
        /// </summary>
        /// <param name="claims">The user's claims.</param>
        public void SetUserEmail(IEnumerable<Claim> claims)
        {
            Contract.Requires(claims != null, "The claims must not be null.");
            var key = EMAIL_KEY;
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

        /// <summary>
        /// Sets the user's given name.
        /// </summary>
        /// <param name="claims">The user claims.</param>
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

        /// <summary>
        /// Sets the user's surname.
        /// </summary>
        /// <param name="claims">The user claims.</param>
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

        /// <summary>
        /// Sets the user's full name.
        /// </summary>
        /// <param name="claims">The user claims.</param>
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

        /// <summary>
        /// Sets the token issue date.
        /// </summary>
        /// <param name="claims">The user claims.</param>
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

        /// <summary>
        /// Sets the token expiration date.
        /// </summary>
        /// <param name="claims">The user claims.</param>
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
                    this.TokenExpirationDate = GetUTCDate(secondsAfterEpoc);
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

        /// <summary>
        /// Sets the token valid date, i.e. the earliest date the token is valid.
        /// </summary>
        /// <param name="claims">The user token.</param>
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

        /// <summary>
        /// Returns true if the user has permission given all permissions.
        /// </summary>
        /// <param name="requestedPermission">The permission to check.</param>
        /// <param name="allUserPermissions">All user permissions.</param>
        /// <returns>True, if the user has the requested permission.</returns>
        public override bool HasPermission(IPermission requestedPermission, IEnumerable<IPermission> allUserPermissions)
        {
            var hasPermission = allUserPermissions.Contains(requestedPermission);
            return hasPermission;
        }

        /// <summary>
        /// Returns the date given the number of seconds elapsed since the Unix Epoch.
        /// </summary>
        /// <param name="secondsAfterEpoch">The number of seconds elapsed since Jan, 1, 1970.</param>
        /// <returns>The date.</returns>
        public DateTime GetUTCDate(int secondsAfterEpoch)
        {
            return EPOCH.AddSeconds((double)secondsAfterEpoch);
        }

        /// <summary>
        /// Returns the user's email.
        /// </summary>
        /// <returns>The user's email.</returns>
        public override string GetUsername()
        {
            return this.Email;
        }
    }
}