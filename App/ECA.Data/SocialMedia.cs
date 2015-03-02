using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{

    public class SocialMedia
    {
        /// <summary>
        /// A social media identity for a participant or organization.
        /// </summary>
        [Key]
        public int SocialMediaId { get; set; }

        public SocialMediaType SocialMediaType { get; set; }
        [Required]
        public int SocialMediaTypeId { get; set; }
        public string SocialMediaValue { get; set; }
        public virtual Organization Organization { get; set; }
        public int? OrganizationId { get; set; }
        public virtual Person Person { get; set; }
        public int? PersonId { get; set; }

        public History History { get; set; }
    }

}
