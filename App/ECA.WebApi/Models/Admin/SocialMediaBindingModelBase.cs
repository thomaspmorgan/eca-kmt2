using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The SocialMediaBindingModelBase is used as a base class when a client is creating or updated a social media.
    /// </summary>
    public abstract class SocialMediaBindingModelBase
    {
        /// <summary>
        /// The social media type id.
        /// </summary>
        public int SocialMediaTypeId { get; set; }

        /// <summary>
        /// The social media value.
        /// </summary>
        [MaxLength(SocialMedia.VALUE_MAX_LENGTH)]
        [Required]
        public string Value { get; set; }
    }

    /// <summary>
    /// The SocialMediaBindingModelBase is used as a base class when a client is creating or updated a social media.
    /// </summary>
    /// <typeparam name="T">The socialable entity type.</typeparam>
    public abstract class SocialMediaBindingModelBase<T> : SocialMediaBindingModelBase
        where T : class, ISocialable
    {
        /// <summary>
        /// The socialable entity id.
        /// </summary>
        public int SocialableId { get; set; }

        /// <summary>
        /// Returns a business layer entity to create or update social medias.
        /// </summary>
        /// <param name="user">The user performing the operation.</param>
        /// <returns>The social media presence.</returns>
        public abstract SocialMediaPresence<T> ToSocialMediaPresence(User user);
    }
}