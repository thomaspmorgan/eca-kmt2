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
    /// A SocialMediaPresence provides a business layer client the ability to add a social media reference
    /// to an ISocialable entity.
    /// </summary>
    public abstract class SocialMediaPresence<T> 
        where T : class, ISocialable
    {
        /// <summary>
        /// Creates a new social media presence with the user, type of social media, and value.
        /// </summary>
        /// <param name="user">The user creating the social media presence.</param>
        /// <param name="socialMediaTypeId">The social media type by id.</param>
        /// <param name="value">The value.</param>
        public SocialMediaPresence(User user, int socialMediaTypeId, string value)
        {
            Contract.Requires(user != null, "The user must not be null.");
            if (SocialMediaType.GetStaticLookup(socialMediaTypeId) == null)
            {
                throw new UnknownStaticLookupException(string.Format("The social media type with id [{0}] is not known.", socialMediaTypeId));
            }
            this.SocialMediaTypeId = socialMediaTypeId;
            this.Value = value;
            this.Create = new Create(user);
        }

        /// <summary>
        /// Gets the social media type id.
        /// </summary>
        public int SocialMediaTypeId { get; private set; }

        /// <summary>
        /// Gets the social media value.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Gets the create audit info.
        /// </summary>
        public Create Create { get; private set; }

        /// <summary>
        /// Adds the given 
        /// </summary>
        /// <param name="socialable">The socialable entity.</param>
        /// <returns>The social media that should be added to the context.</returns>
        public SocialMedia AddSocialMediaPresence(ISocialable socialable)
        {
            Contract.Requires(socialable != null, "The socialable entity must not be null.");
            Contract.Requires(socialable.SocialMedias != null, "The social medias property must not be null.");
            var socialMedia = new SocialMedia
            {
                SocialMediaTypeId = this.SocialMediaTypeId,
                SocialMediaValue = this.Value
            };
            this.Create.SetHistory(socialMedia);
            socialable.SocialMedias.Add(socialMedia);
            return socialMedia;
        }

        public abstract int GetSocialableEntityId();
    }
}
