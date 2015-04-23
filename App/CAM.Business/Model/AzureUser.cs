using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Business.Model
{
    /// <summary>
    /// An AzureUser is a user that has been registered through Microsoft Azure.  This model can be used for creating
    /// or updating users in CAM.
    /// </summary>
    public class AzureUser
    {
        /// <summary>
        /// Creates a new AzureUser instance.
        /// </summary>
        /// <param name="id">The id of the azure user.</param>
        /// <param name="email">The email address of the user.</param>
        /// <param name="firstName">The first name of the user.</param>
        /// <param name="lastName">The last name of the user.</param>
        /// <param name="displayName">The display name of the user.</param>
        public AzureUser(Guid id, string email, string firstName, string lastName, string displayName)
        {
            this.Id = id;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DisplayName = displayName;
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the user email.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName { get; private set; }
    }
}
