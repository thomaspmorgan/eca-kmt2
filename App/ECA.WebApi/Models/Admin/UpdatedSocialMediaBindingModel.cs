using ECA.Business.Service;
using ECA.Business.Service.Admin;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The UpdatedSocialMediaBindingModel is used when an api client wishes to update
    /// a social media presence.
    /// </summary>
    public class UpdatedSocialMediaBindingModel : SocialMediaBindingModelBase
    {
        /// <summary>
        /// The id of the social media presence.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Returns a business layer entity to update a social media presence.
        /// </summary>
        /// <param name="updator">The user performing the update.</param>
        /// <returns>The UpdatedSocialMediaPresence instance.</returns>
        public UpdatedSocialMediaPresence ToUpdatedSocialMediaPresence(User updator)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            return new UpdatedSocialMediaPresence(updator, this.Id, this.Value, this.SocialMediaTypeId);
        }
    }
}