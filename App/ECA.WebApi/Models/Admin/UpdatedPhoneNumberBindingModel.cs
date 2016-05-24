using ECA.Business.Service;
using ECA.Business.Service.Admin;
using System.Diagnostics.Contracts;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The UpdatedEmailAddressBindingModel is used when an api client wishes to update
    /// an email address.
    /// </summary>
    public class UpdatedPhoneNumberBindingModel : PhoneNumberBindingModelBase
    {
        /// <summary>
        /// The id of the phone number.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Returns a business layer entity to update a email address.
        /// </summary>
        /// <param name="updator">The user performing the update.</param>
        /// <returns>The UpdatedEmailAddress instance.</returns>
        public UpdatedPhoneNumber ToUpdatedPhoneNumber(User updator)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            return new UpdatedPhoneNumber(
                updator: updator,
                id: this.Id,
                number: this.Number,
                extension: this.Extension,
                phoneNumberTypeId: this.PhoneNumberTypeId,
                isPrimary: this.IsPrimary);

        }
    }
}