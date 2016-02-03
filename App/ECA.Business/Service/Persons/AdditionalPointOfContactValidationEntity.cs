using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An AdditionalPointOfContactValidationEntity is used to validate a new point of contact.
    /// </summary>
    public class AdditionalPointOfContactValidationEntity
    {
        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="fullName">The full name of the point of the contact.</param>
        /// <param name="position">The position of the point of contact.</param>
        /// <param name="likeEmailAddressCount">The number of email addresses that are equal to the point of contact's email address.</param>
        public AdditionalPointOfContactValidationEntity(string fullName, string position, int likeEmailAddressCount)
        {
            this.FullName = fullName;
            this.Position = position;
            this.LikeEmailAddressCount = likeEmailAddressCount;
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        public string Position { get; private set; }

        /// <summary>
        /// Gets the like email address count.
        /// </summary>
        public int LikeEmailAddressCount { get; private set; }
    }
}
