using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// An UpdatedSocialMediaPresense is used by a business layer client to update a social media entity.
    /// </summary>
    public class UpdatedSocialMediaPresence
    {
        /// <summary>
        /// Creates a new UpdatedSocialMediaPresence and initialized it with the given values.
        /// </summary>
        /// <param name="updator">The updator.</param>
        /// <param name="id">The id of the social media presence.</param>
        /// <param name="value">The value.</param>
        /// <param name="socialMediaTypeId">The social media type id.</param>
        public UpdatedSocialMediaPresence(User updator, int id, string value, int socialMediaTypeId)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            if (SocialMediaType.GetStaticLookup(socialMediaTypeId) == null)
            {
                throw new UnknownStaticLookupException(string.Format("The social media type with id [{0}] is not known.", socialMediaTypeId));
            }
            this.Update = new Update(updator);
            this.Id = id;
            this.Value = value;
            this.SocialMediaTypeId = socialMediaTypeId;            
        }

        /// <summary>
        /// Gets the update audit.
        /// </summary>
        public Update Update { get; private set; }

        /// <summary>
        /// Gets the Id.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Gets the social media type id.
        /// </summary>
        public int SocialMediaTypeId { get; private set; }
    }
}
