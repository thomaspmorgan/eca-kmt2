using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service
{
    /// <summary>
    /// The User is a container for holding information about a user when interacting with the business model.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userId">The user id</param>
        public User(int userId)
        {
            this.Id = userId;
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; private set; }

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
            return this.Id == otherType.Id;

        }

        /// <summary>
        /// Returns a hash of this object.
        /// </summary>
        /// <returns>A hash of this object.</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
