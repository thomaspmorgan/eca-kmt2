using ECA.Data;
using System.Linq;

namespace ECA.Business.Service.Admin
{
    public class NewContactPhoneNumber : NewPhoneNumber<Contact>
    {
        /// <summary>
        /// Creates a new phone number with the user, type of phone number, and number.
        /// </summary>
        /// <param name="user">The user creating the phone number.</param>
        /// <param name="phoneNumberTypeId">The  phone number type by id.</param>
        /// <param name="number">The value.</param>
        /// <param name="contactId">The person id.</param>
        /// <param name="isPrimary">The is primary phone number flag.</param>
        public NewContactPhoneNumber(User user, int phoneNumberTypeId, string number, string extension, int contactId, bool isPrimary)
            :base(user, phoneNumberTypeId, number, extension, isPrimary)
        {
            this.ContactId = contactId;
        }

        /// <summary>
        /// Gets the organization id.
        /// </summary>
        public int ContactId { get; private set; }

        /// <summary>
        /// Returns a query to retrieve phone numbers of the person with this instance's ContactId.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns></returns>
        public override IQueryable<PhoneNumber> CreateGetPhoneNumbersQuery(EcaContext context)
        {
            return context.PhoneNumbers.Where(x => x.ContactId == this.ContactId);
        }

        /// <summary>
        /// Returns the person id.
        /// </summary>
        /// <returns>The person id.</returns>
        public override int GetPhoneNumberableEntityId()
        {
            return this.ContactId;
        }
    }
}
