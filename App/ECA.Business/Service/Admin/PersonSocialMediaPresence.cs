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
    public class PersonSocialMediaPresence : SocialMediaPresence<Person>
    {
        /// <summary>
        /// Creates a new social media presence with the user, type of social media, and value.
        /// </summary>
        /// <param name="user">The user creating the social media presence.</param>
        /// <param name="socialMediaTypeId">The social media type by id.</param>
        /// <param name="value">The value.</param>
        /// <param name="personId">The person id.</param>
        public PersonSocialMediaPresence(User user, int socialMediaTypeId, string value, int personId)
            :base(user, socialMediaTypeId, value)
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
        public override int GetSocialableEntityId()
        {
            return this.PersonId;
        }
    }
}
