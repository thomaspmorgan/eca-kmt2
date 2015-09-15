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
}