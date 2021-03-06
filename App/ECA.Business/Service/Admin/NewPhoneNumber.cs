﻿using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Service.Admin
{
    public class NewPhoneNumber : IAuditable
    {
        /// <summary>
        /// Creates a new phone number with the user, and phone number value.
        /// </summary>
        /// <param name="user">The user creating the phone number.</param>
        /// <param name="phoneNumberTypeId">The phone number type of the phone number</param>
        /// <param name="number">The phone number value.</param>
        /// <param name="extension">The phone number extension value.</param>
        /// <param name="isPrimary">The is primary phone number flag.</param>
        public NewPhoneNumber(User user, int phoneNumberTypeId, string number, string extension, bool isPrimary)
        {
            Contract.Requires(user != null, "The user must not be null.");
            this.PhoneNumberTypeId = phoneNumberTypeId;
            this.Number = number;
            this.Extension = extension;
            this.IsPrimary = isPrimary;
            this.Audit = new Create(user);
        }

        /// <summary>
        /// Gets the phone number value.
        /// </summary>
        public string Number { get; private set; }

        /// <summary>
        /// Gets or sets the phone number extension.
        /// </summary>
        public string Extension { get; private set; }

        /// <summary>
        /// Gets the phone number value.
        /// </summary>
        public int PhoneNumberTypeId { get; private set; }

        /// <summary>
        /// Gets the create audit info.
        /// </summary>
        public Audit Audit { get; private set; }

        /// <summary>
        /// Gets whether or not this phone number is the primary phone number.
        /// </summary>
        public bool IsPrimary { get; private set; }
    }

    /// <summary>
    /// A NewEmailAddress provides a business layer client the ability to add a phone number
    /// to an IPhoneNumberable entity.
    /// </summary>
    public abstract class NewPhoneNumber<T> : NewPhoneNumber
        where T : class, IPhoneNumberable
    {
        /// <summary>
        /// Creates a new phone number with the user, and phone number value.
        /// </summary>
        /// <param name="user">The user creating the phone number.</param>
        /// <param name="phoneNumberTypeId">The phone number type of the phone number</param>
        /// <param name="number">The phone number value.</param>
        /// <param name="extension">The phone number extension value.</param>
        /// <param name="isPrimary">The is primary phone number flag.</param>
        public NewPhoneNumber(User user, int phoneNumberTypeId, string number, string extension, bool isPrimary)
            : base(user, phoneNumberTypeId, number, extension, isPrimary)
        {
            Contract.Requires(user != null, "The user must not be null.");
        }

        /// <summary>
        /// Adds the given 
        /// </summary>
        /// <param name="phoneNumberable">The IPhoneNumberable entity.</param>
        /// <returns>The phone number that should be added to the context.</returns>
        public PhoneNumber AddPhoneNumber(T phoneNumberable)
        {
            Contract.Requires(phoneNumberable != null, "The addressable entity must not be null.");
            Contract.Requires(phoneNumberable.PhoneNumbers != null, "The phone numbers property must not be null.");
            var phoneNumber = new PhoneNumber
            {
                Number = this.Number,
                Extension = this.Extension,
                PhoneNumberTypeId = this.PhoneNumberTypeId,
                IsPrimary = this.IsPrimary
            };
            Contract.Assert(this.Audit.GetType() == typeof(Create), "The audit details must be a Create type.");
            this.Audit.SetHistory(phoneNumber);
            phoneNumberable.PhoneNumbers.Add(phoneNumber);
            return phoneNumber;
        }

        /// <summary>
        /// abstract method that returns the id of the PhoneNumberable entity
        /// </summary>
        /// <returns></returns>
        public abstract int GetPhoneNumberableEntityId();

        /// <summary>
        /// Creates a query to retrieve all phone numbers for the entity of type T.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query.</returns>
        public abstract IQueryable<PhoneNumber> CreateGetPhoneNumbersQuery(EcaContext context);
    }
}
