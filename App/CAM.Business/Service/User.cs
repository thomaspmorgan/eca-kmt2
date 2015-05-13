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

        public int PrincipalId { get; set; }
        public string DisplayName { get; set; }
        public String AccountStatusText { get; set; }
        
        public int AccountStatusId { get; set; }
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