using ECA.Business.Service;
using ECA.Business.Service.Admin;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The UpdatedEmailAddressBindingModel is used when an api client wishes to update
    /// an email address.
    /// </summary>
    public class UpdatedEmailAddressBindingModel : EmailAddressBindingModelBase
    {
        /// <summary>
        /// The id of the email address.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Returns a business layer entity to update a email address.
        /// </summary>
        /// <param name="updator">The user performing the update.</param>
        /// <returns>The UpdatedEmailAddress instance.</returns>
        public UpdatedEmailAddress ToUpdatedEmailAddress(User updator)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            return new UpdatedEmailAddress(
                updator: updator,
                id: this.Id,
                address: this.Address,
                emailAddressTypeId: this.EmailAddressTypeId,
                isPrimary: this.IsPrimary);

        }
    }
}