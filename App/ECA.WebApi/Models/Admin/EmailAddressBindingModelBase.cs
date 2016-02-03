using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The EmailAddressBindingModelBase is used as a base class when a client is creating or updated a email address.
    /// </summary>
    public abstract class EmailAddressBindingModelBase
    {
        /// <summary>
        /// The social media type id.
        /// </summary>
        public int EmailAddressTypeId { get; set; }

        /// <summary>
        /// The social media value.
        /// </summary>
        [MaxLength(EmailAddress.EMAIL_ADDRESS_MAX_LENGTH)]
        [Required]
        public string Address { get; set; }
    }

    /// <summary>
    /// The EmailAddressBindingModelBase is used as a base class when a client is creating or updated an email address.
    /// </summary>
    /// <typeparam name="T">The emailAddressable entity type.</typeparam>
    public abstract class EmailAddressBindingModelBase<T> : EmailAddressBindingModelBase
        where T : class, IEmailAddressable
    {
        /// <summary>
        /// The socialable entity id.
        /// </summary>
        public int EMailAddressableId { get; set; }

        /// <summary>
        /// Returns a business layer entity to create or update an email address.
        /// </summary>
        /// <param name="user">The user performing the operation.</param>
        /// <returns>The email address.</returns>
        public abstract NewEmailAddress<T> ToEmailAddress(User user);
    }

    /// <summary>
    /// An AdditionalEmailAddressBindingModel is used when the email address that is created will
    /// be related to another model implicity such as creating a new point of a contact.
    /// </summary>
    public class AdditionalEmailAddressBindingModel : EmailAddressBindingModelBase
    {
        /// <summary>
        /// Returns a new NewEmailAddress business entity.
        /// </summary>
        /// <param name="user">The user creating the address.</param>
        /// <returns>The new email address.</returns>
        public NewEmailAddress ToNewEmailAddress(User user)
        {
            return new NewEmailAddress(
                user: user,
                emailAddressTypeId: this.EmailAddressTypeId,
                address: this.Address
                );
        }
    }
}