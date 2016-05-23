using ECA.Business.Service.Admin;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The ContactEmailAddressBindingModel is used when a client is creating or updated an email address on a contact.
    /// </summary>
    public class ContactEmailAddressBindingModel : EmailAddressBindingModelBase<ECA.Data.Contact>
    {
        /// <summary>
        /// Returns a business layer entity to create or update email addreses.
        /// </summary>
        /// <param name="user">The user performing the operation.</param>
        /// <returns>The email address.</returns>
        public override NewEmailAddress<ECA.Data.Contact> ToEmailAddress(Business.Service.User user)
        {
            return new NewContactEmailAddress(user, this.EmailAddressTypeId, this.Address, this.IsPrimary, this.EMailAddressableId);
        }
        
    }
}