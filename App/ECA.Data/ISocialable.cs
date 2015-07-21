using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// An ISocialable entity is an entity that maintains a social media presence, such as facebook or twitter.
    /// </summary>
    public interface ISocialable
    {
        /// <summary>
        /// Gets or sets the social media presence of this entity.
        /// </summary>
        ICollection<SocialMedia> SocialMedias { get; set; }
    }
}
