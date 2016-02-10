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
    public class NewEmailAddress : IAuditable
    {
        /// <summary>
        /// Creates a new email address with the user, and address value.
        /// </summary>
        /// <param name="user">The user creating the social media presence.</param>
        /// <param name="emailAddressTypeId">The email address type of the email</param>
        /// <param name="address">The email address value.</param>
        public NewEmailAddress(User user, int emailAddressTypeId, string address, bool isPrimary)
        {
            Contract.Requires(user != null, "The user must not be null.");
            this.EmailAddressTypeId = emailAddressTypeId;
            this.Address = address;
            this.IsPrimary = isPrimary;
            this.Audit = new Create(user);
        }

        /// <summary>
        /// Gets the email address value.
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Gets the email address value.
        /// </summary>
        public int EmailAddressTypeId { get; private set; }

        /// <summary>
        /// Gets whether the email address is primary.
        /// </summary>
        public bool IsPrimary { get; private set; }

        /// <summary>
        /// Gets the create audit info.
        /// </summary>
        public Audit Audit { get; private set; }
    }


    /// <summary>
    /// A NewEmailAddress provides a business layer client the ability to add an email address
    /// to an IEmailAddressable entity.
    /// </summary>
    public abstract class NewEmailAddress<T> : NewEmailAddress
        where T : class, IEmailAddressable
    {
        /// <summary>
        /// Creates a new email address with the user, and address value.
        /// </summary>
        /// <param name="user">The user creating the social media presence.</param>
        /// <param name="emailAddressTypeId">The email address type of the email</param>
        /// <param name="address">The email address value.</param>
        public NewEmailAddress(User user, int emailAddressTypeId, string address, bool isPrimary)
            : base(user, emailAddressTypeId, address, isPrimary)
        {
            Contract.Requires(user != null, "The user must not be null.");
        }

        /// <summary>
        /// Adds the given address to the addressable entity.
        /// </summary>
        /// <param name="addressable">The socialable entity.</param>
        /// <returns>The email address that should be added to the context.</returns>
        public EmailAddress AddEmailAddress(T addressable)
        {
            Contract.Requires(addressable != null, "The addressable entity must not be null.");
            Contract.Requires(addressable.EmailAddresses != null, "The email address property must not be null.");
            var emailAddress = new EmailAddress
            {
                Address = this.Address,
                EmailAddressTypeId = this.EmailAddressTypeId,
                IsPrimary = this.IsPrimary  
            };
            Contract.Assert(this.Audit.GetType() == typeof(Create), "The audit details must be a Create type.");
            this.Audit.SetHistory(emailAddress);
            addressable.EmailAddresses.Add(emailAddress);
            return emailAddress;
        }

        /// <summary>
        /// abstract method that returns the id of the Email Addressable entity
        /// </summary>
        /// <returns></returns>
        public abstract int GetEmailAddressableEntityId();

        /// <summary>
        /// Creates a query to retrieve all email addresses for the entity of type T.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query.</returns>
        public abstract IQueryable<EmailAddress> CreateGetEmailAddressesQuery(EcaContext context);
    }
}
