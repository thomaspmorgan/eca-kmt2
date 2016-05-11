using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Persons;
using System.Collections.Generic;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// An UpdatedPointOfContactBindingModel is used by a web api client to update a project's point of contact details.
    /// </summary>
    public class UpdatedPointOfContactBindingModel
    {
        /// <summary>
        /// The id of the point of contact
        /// </summary>
        public int ContactId { get; set; }

        /// <summary>
        /// The full name of the point of contact.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The position of the point of contact.
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// The email addresses to add to the point of contact.
        /// </summary>
        public IEnumerable<UpdatedEmailAddress> EmailAddresses { get; set; }

        /// <summary>
        /// The phone numbers to add to the point of contact.
        /// </summary>
        public IEnumerable<UpdatedPhoneNumber> PhoneNumbers { get; set; }

        /// <summary>
        /// Returns a business layer UpdatedPointOfContact instance.
        /// </summary>
        /// <param name="user">The user performing the update.</param>
        /// <returns>The business layer update entity.</returns>
        public UpdatedPointOfContact ToUpdatePointOfContact(User user)
        {
            var model = new UpdatedPointOfContact(
                updater: user,
                contactId: ContactId,
                fullName: FullName,
                position: Position,
                emailAddresses: EmailAddresses,
                phoneNumbers: PhoneNumbers);
            return model;
        }
    }
}