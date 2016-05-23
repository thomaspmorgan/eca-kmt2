using ECA.Business.Service.Admin;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The PersonPhoneNumberBindingModel is used when a client is creating or updated an phone numbers on a person.
    /// </summary>
    public class PersonPhoneNumberBindingModel : PhoneNumberBindingModelBase<ECA.Data.Person>
    {
        /// <summary>
        /// Returns a business layer entity to create or update phone numbers.
        /// </summary>
        /// <param name="user">The user performing the operation.</param>
        /// <returns>The phone numbers.</returns>
        public override NewPhoneNumber<ECA.Data.Person> ToPhoneNumber(Business.Service.User user)
        {
            return new NewPersonPhoneNumber(user, this.PhoneNumberTypeId, this.Number, this.Extension, this.PhoneNumberableId, this.IsPrimary);
        }
    }
}