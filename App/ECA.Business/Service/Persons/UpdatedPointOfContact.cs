using ECA.Business.Service.Admin;
using System.Collections.Generic;

namespace ECA.Business.Service.Persons
{
    public class UpdatedPointOfContact : IAuditable
    {
        /// <summary>
        /// Creates a new UpdatedPointOfContact instance.
        /// </summary>
        /// <param name="updater">The user updating the poc</param>
        /// <param name="contactId">The id of the poc to be updated</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="position">The position.</param>
        /// <param name="emailAddresses">The collection of email addresses.</param>
        /// <param name="phoneNumbers">The phone numbers.</param>
        public UpdatedPointOfContact(User updater,
            int contactId,
            string fullName,
            string position,
            IEnumerable<UpdatedEmailAddress> emailAddresses,
            IEnumerable<UpdatedPhoneNumber> phoneNumbers)
        {
            this.ContactId = contactId;
            this.FullName = fullName;
            this.Position = position;
            this.EmailAddresses = emailAddresses ?? new List<UpdatedEmailAddress>();
            this.PhoneNumbers = phoneNumbers ?? new List<UpdatedPhoneNumber>();
            this.Audit = new Update(updater);
        }

        /// <summary>
        /// Gets or sets the contact id.
        /// </summary>
        public int ContactId { get; set; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        public string Position { get; private set; }

        /// <summary>
        /// Gets the email addresses.
        /// </summary>
        public IEnumerable<UpdatedEmailAddress> EmailAddresses { get; private set; }

        /// <summary>
        /// Gets the phone numbers.
        /// </summary>
        public IEnumerable<UpdatedPhoneNumber> PhoneNumbers { get; private set; }

        /// <summary>
        /// Gets the audit.
        /// </summary>
        public Audit Audit { get; private set; }

    }
}
