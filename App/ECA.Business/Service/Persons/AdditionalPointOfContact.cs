using ECA.Business.Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public class AdditionalPointOfContact : IAuditable
    {
        /// <summary>
        /// Creates a new AdditionalPointOfContact instance.
        /// </summary>
        /// <param name="creator">The user creating the poc.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="position">The position.</param>
        /// <param name="emailAddresses">The collection of email addresses.</param>
        /// <param name="phoneNumbers">The phone numbers.</param>
        public AdditionalPointOfContact(User creator, string fullName, string position, IEnumerable<NewEmailAddress> emailAddresses, IEnumerable<NewPhoneNumber> phoneNumbers)
        {
            this.FullName = fullName;
            this.Position = position;
            this.EmailAddresses = emailAddresses ?? new List<NewEmailAddress>();
            this.PhoneNumbers = phoneNumbers ?? new List<NewPhoneNumber>();
            this.Audit = new Create(creator);
        }

        /// <summary>
        /// Gets the audit.
        /// </summary>
        public Audit Audit { get; private set; }

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
        public IEnumerable<NewEmailAddress> EmailAddresses { get; private set; }

        /// <summary>
        /// Gets the phone numbers.
        /// </summary>
        public IEnumerable<NewPhoneNumber> PhoneNumbers { get; private set; }
    }
}
