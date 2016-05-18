using ECA.Data;
using System.Linq;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Allows a business layer client to add a phone number to a person.
    /// </summary>
    public class NewPersonPhoneNumber : NewPhoneNumber<Person>
    {
        /// <summary>
        /// Creates a new phone number with the user, type of phone number, and number.
        /// </summary>
        /// <param name="user">The user creating the phone number.</param>
        /// <param name="phoneNumberTypeId">The  phone number type by id.</param>
        /// <param name="number">The value.</param>
        /// <param name="personId">The person id.</param>
        /// <param name="isPrimary">The is primary phone number flag.</param>
        public NewPersonPhoneNumber(User user, int phoneNumberTypeId, string number, string extension, int personId, bool isPrimary)
            :base(user, phoneNumberTypeId, number, extension, isPrimary)
        {
            this.PersonId = personId;
        }

        /// <summary>
        /// Gets the organization id.
        /// </summary>
        public int PersonId { get; private set; }

        /// <summary>
        /// Returns a query to retrieve phone numbers of the person with this instance's PersonId.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns></returns>
        public override IQueryable<PhoneNumber> CreateGetPhoneNumbersQuery(EcaContext context)
        {
            return context.PhoneNumbers.Where(x => x.PersonId == this.PersonId);
        }

        /// <summary>
        /// Returns the person id.
        /// </summary>
        /// <returns>The person id.</returns>
        public override int GetPhoneNumberableEntityId()
        {
            return this.PersonId;
        }
    }
}
