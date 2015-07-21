using ECA.Business.Service.Admin;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The OrganizationSocialMediaPresenceBindingModel is used when a client is creating or updated a social media on an organization.
    /// </summary>
    public class OrganizationSocialMediaPresenceBindingModel : SocialMediaBindingModelBase<Organization>
    {
        /// <summary>
        /// Returns a business layer entity to create or update social medias.
        /// </summary>
        /// <param name="user">The user performing the operation.</param>
        /// <returns>The social media presence.</returns>
        public override SocialMediaPresence<Organization> ToSocialMediaPresence(Business.Service.User user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return new OrganizationSocialMediaPresence(user, this.SocialMediaTypeId, this.Value, this.Id);
        }
    }
}