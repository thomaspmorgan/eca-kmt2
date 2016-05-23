using ECA.Business.Service.Admin;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The ContactPhoneNumberBindingModel is used when a client is creating or updated an phone numbers on a contact.
    /// </summary>
    public class ContactPhoneNumberBindingModel : PhoneNumberBindingModelBase<ECA.Data.Contact>
    {
        /// <summary>
        /// Returns a business layer entity to create or update phone numbers.
        /// </summary>
        /// <param name="user">The user performing the operation.</param>
        /// <returns>The phone numbers.</returns>
        public override NewPhoneNumber<ECA.Data.Contact> ToPhoneNumber(Business.Service.User user)
        {
            return new NewContactPhoneNumber(user, this.PhoneNumberTypeId, this.Number, this.Extension, this.PhoneNumberableId, this.IsPrimary);
        }
        
    }
}