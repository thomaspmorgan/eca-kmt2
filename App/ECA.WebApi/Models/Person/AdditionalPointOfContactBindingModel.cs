using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Persons;
using ECA.WebApi.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// The AdditionalPointOfContactBindingModel is used to create a new point of contact in the system.  The phone numbers
    /// and email addresses will also be added to the point of contact.
    /// </summary>
    public class AdditionalPointOfContactBindingModel
    {
        /// <summary>
        /// Creates a new instance and initializes the email addresses and phone numbers.
        /// </summary>
        public AdditionalPointOfContactBindingModel()
        {
            this.EmailAddresses = new List<AdditionalEmailAddressBindingModel>();
            this.PhoneNumbers = new List<AdditionalPhoneNumberBindingModel>();
        }

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
        public IEnumerable<AdditionalEmailAddressBindingModel> EmailAddresses { get; set; }

        /// <summary>
        /// The phone numbers to add to the point of contact.
        /// </summary>
        public IEnumerable<AdditionalPhoneNumberBindingModel> PhoneNumbers { get; set; }

        /// <summary>
        /// Returns an AdditionalPointOfContact business entity to be used in the business layer.
        /// </summary>
        /// <param name="user">The user creating the point of contact.</param>
        /// <returns>The business entity instance.</returns>
        public AdditionalPointOfContact ToAdditionalPointOfContact(User user)
        {
            var newEmailAddresses = this.EmailAddresses.Select(x => x.ToNewEmailAddress(user)).ToList();
            var newPhoneNumbers = this.PhoneNumbers.Select(x => x.ToNewPhoneNumber(user)).ToList();

            return new AdditionalPointOfContact(
                creator: user,
                fullName: this.FullName,
                position: this.Position,
                emailAddresses: newEmailAddresses,
                phoneNumbers: newPhoneNumbers
                );
        }
    }
}