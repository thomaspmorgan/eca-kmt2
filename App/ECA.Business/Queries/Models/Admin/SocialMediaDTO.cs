using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// A SocialMediaDTO is used to represent a social media entity in the ECA System.
    /// </summary>
    public class SocialMediaDTO
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the social media type id.
        /// </summary>
        public int SocialMediaTypeId { get; set; }

        /// <summary>
        /// Gets or sets the social media type.
        /// </summary>
        public string SocialMediaType { get; set; }

        /// <summary>
        /// Gets or sets the social media value.
        /// </summary>
        public string SocialMediaValue { get; set; }
    }
}
