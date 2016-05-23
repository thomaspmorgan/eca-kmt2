using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Data;
using System.ComponentModel.DataAnnotations;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The PhoneNumberBindingModelBase is used as a base class when a client is creating or updated a phone number.
    /// </summary>
    public abstract class PhoneNumberBindingModelBase
    {
        /// <summary>
        /// The phone number type id.
        /// </summary>
        public int PhoneNumberTypeId { get; set; }

        /// <summary>
        /// The phone number value.
        /// </summary>
        [Required]
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the phone number extension.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// If true, this phone number is the primary phone number.
        /// </summary>
        public bool IsPrimary { get; set; }
    }

    /// <summary>
    /// The PhoneNumberBindingModelBase is used as a base class when a client is creating or updated a phone number.
    /// </summary>
    /// <typeparam name="T">The IPhoneNumberable entity type.</typeparam>
    public abstract class PhoneNumberBindingModelBase<T> : PhoneNumberBindingModelBase
        where T : class, IPhoneNumberable
    {
        /// <summary>
        /// The phoneNumberable entity id.
        /// </summary>
        public int PhoneNumberableId { get; set; }

        /// <summary>
        /// Returns a business layer entity to create or update a phone number.
        /// </summary>
        /// <param name="user">The user performing the operation.</param>
        /// <returns>The phone number.</returns>
        public abstract NewPhoneNumber<T> ToPhoneNumber(User user);
    }

    /// <summary>
    /// An AdditionalPhoneNumberbindingModel is used when the phone number is related to another
    /// model implicity, such as creating a new point of contact.
    /// </summary>
    public class AdditionalPhoneNumberBindingModel : PhoneNumberBindingModelBase
    {
        /// <summary>
        /// Returns a new NewPhoneNumber business entity.
        /// </summary>
        /// <param name="user">the user creating the phone number.</param>
        /// <returns>The business entity instance.</returns>
        public NewPhoneNumber ToNewPhoneNumber(User user)
        {
            return new NewPhoneNumber(
                user: user,
                phoneNumberTypeId: this.PhoneNumberTypeId,
                number: this.Number,
                extension: this.Extension,
                isPrimary: this.IsPrimary
                );
        }
    }
}