using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Allows a business layer client to add a social media presence to an organization.
    /// </summary>
    public class OrganizationSocialMediaPresence : SocialMediaPresence<Organization>
    {
        /// <summary>
        /// Creates a new social media presence with the user, type of social media, and value.
        /// </summary>
        /// <param name="user">The user creating the social media presence.</param>
        /// <param name="socialMediaTypeId">The social media type by id.</param>
        /// <param name="value">The value.</param>
        /// <param name="organizationId">The organization id.</param>
        public OrganizationSocialMediaPresence(User user, int socialMediaTypeId, string value, int organizationId)
            :base(user, socialMediaTypeId, value)
        {
            this.OrganizationId = organizationId;
        }

        /// <summary>
        /// Gets the organization id.
        /// </summary>
        public int OrganizationId { get; private set; }

        /// <summary>
        /// Returns the organization id.
        /// </summary>
        /// <returns>The organization id.</returns>
        public override int GetSocialableEntityId()
        {
            return this.OrganizationId;
        }
    }
}
