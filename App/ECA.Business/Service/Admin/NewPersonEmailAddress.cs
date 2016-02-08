using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Allows a business layer client to add a social media presence to a person.
    /// </summary>
    public class NewPersonEmailAddress : NewEmailAddress<Person>
    {
        /// <summary>
        /// Creates a new email address with the user, type of email address, and address.
        /// </summary>
        /// <param name="user">The user creating the social media presence.</param>
        /// <param name="emailAddressTypeId">The email address type by id.</param>
        /// <param name="address">The value.</param>
        /// <param name="personId">The person id.</param>
        /// <param name="isPrimary">The is primary flag.</param>
        public NewPersonEmailAddress(User user, int emailAddressTypeId, string address, bool isPrimary, int personId)
            :base(user, emailAddressTypeId, address, isPrimary)
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
        public override int GetEmailAddressableEntityId()
        {
            return this.PersonId;
        }
    }
}
