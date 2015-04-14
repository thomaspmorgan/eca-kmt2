using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAM.Data;

namespace CAM.Business.Service
{
    /// <summary>
    /// The User is a container for holding information about a user when interacting with the business model.
    /// </summary>
    public class User
    {

        private CamModel cam = new CamModel();

        public int PrincipalId { get; set; }
        public string DisplayName { get; set; }
        public String AccountStatusText { get; set; }
        public Enums.AccountStatus AccountStatus { get; set; }
        public DateTimeOffset? LastAccessed { get; set; }
        public DateTimeOffset? SuspendedDate { get; set; }
        public DateTimeOffset? RestoredDate { get; set; }
        public DateTimeOffset? RevokedDate { get; set; }
        public DateTimeOffset? ExpiredDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid AdGuid { get; set; }
        public string EmailAddress { get; set; }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        public User()
        {

        }

        public bool AuthenticateUserWithGuid(Guid userGuid)
        {
                var results = (from r in cam.UserAccounts
                               where r.AdGuid == userGuid
                               select r).FirstOrDefault();

                if (results != null)
                {
                    //if (results.ExpirationDate > DateTime.Now && results.AccountStatusId == (int)Enums.AccountStatus.Active)
                    if(results.AccountStatusId == (int)Enums.AccountStatus.Active)
                    {
                        // update last accessed property
                        results.LastAccessed = DateTime.Now;
                        cam.SaveChanges();
                    }
                    // fill in User properties
                    PrincipalId = results.PrincipalId;
                    LastAccessed = results.LastAccessed;
                    SuspendedDate = results.SuspendedDate;
                    RevokedDate = results.RevokedDate;
                    RestoredDate = results.RestoredDate;
                    ExpiredDate = results.ExpiredDate;
                    if (ExpiredDate > DateTime.Now)
                    {
                        AccountStatusText = results.AccountStatus.Status;
                        AccountStatus = (Enums.AccountStatus)results.AccountStatusId;
                    }
                    else
                    {
                        AccountStatusText = Enums.AccountStatus.Expired.ToString();
                        AccountStatus = Enums.AccountStatus.Expired;
                    }

                    FirstName = results.FirstName;
                    LastName = results.LastName;
                    DisplayName = results.DisplayName;
                    EmailAddress = results.EmailAddress;
                    // return true means user found, check status to see if active.
                    return true;
                }
                return false; // user not found
        }

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
            var otherType = obj as User;
            if (otherType == null)
            {
                return false;
            }
            return this.PrincipalId == otherType.PrincipalId;

        }

        /// <summary>
        /// Returns a hash of this object.
        /// </summary>
        /// <returns>A hash of this object.</returns>
        public override int GetHashCode()
        {
            return this.PrincipalId.GetHashCode();
        }
    }
}