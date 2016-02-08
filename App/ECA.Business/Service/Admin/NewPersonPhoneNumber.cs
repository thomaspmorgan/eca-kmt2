using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public NewPersonPhoneNumber(User user, int phoneNumberTypeId, string number, int personId, bool isPrimary)
            :base(user, phoneNumberTypeId, number, isPrimary)
        {
            this.PersonId = personId;
        }

        /// <summary>
        /// Gets the organization id.
        /// </summary>
        public int PersonId { get; private set; }

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
