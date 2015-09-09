using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// A NewEmailAddress provides a business layer client the ability to add a phone number
    /// to an IPhoneNumberable entity.
    /// </summary>
    public abstract class NewPhoneNumber<T> 
        where T : class, IPhoneNumberable
    {
        /// <summary>
        /// Creates a new phone number with the user, and phone number value.
        /// </summary>
        /// <param name="user">The user creating the phone number.</param>
        /// <param name="phoneNumberTypeId">The phone number type of the phone number</param>
        /// <param name="number">The phone number value.</param>
        public NewPhoneNumber(User user, int phoneNumberTypeId, string number)
        {
            Contract.Requires(user != null, "The user must not be null.");
            this.PhoneNumberTypeId = phoneNumberTypeId;
            this.Number = number;
            this.Create = new Create(user);
        }

        /// <summary>
        /// Gets the phone number value.
        /// </summary>
        public string Number { get; private set; }

        /// <summary>
        /// Gets the phone number value.
        /// </summary>
        public int PhoneNumberTypeId { get; private set; }

        /// <summary>
        /// Gets the create audit info.
        /// </summary>
        public Create Create { get; private set; }

        /// <summary>
        /// Adds the given 
        /// </summary>
        /// <param name="phoneNumberable">The IPhoneNumberable entity.</param>
        /// <returns>The phone number that should be added to the context.</returns>
        public PhoneNumber AddEmailAddress(IPhoneNumberable phoneNumberable)
        {
            Contract.Requires(phoneNumberable != null, "The addressable entity must not be null.");
            Contract.Requires(phoneNumberable.PhoneNumbers != null, "The phone numbers property must not be null.");
            var phoneNumber = new PhoneNumber
            {
                Number = this.Number,
                PhoneNumberTypeId = this.PhoneNumberTypeId  
            };
            this.Create.SetHistory(phoneNumber);
            phoneNumberable.PhoneNumbers.Add(phoneNumber);
            return phoneNumber;
        }

        /// <summary>
        /// abstract method that returns the id of the PhoneNumberable entity
        /// </summary>
        /// <returns></returns>
        public abstract int GetPhoneNumberableEntityId();
    }
}
