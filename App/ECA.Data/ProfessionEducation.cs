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

    public class ProfessionEducation
    {
        [Key]
        public int ProfessionEducationId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Role { get; set; }
        public virtual Organization Organization { get; set; }
        public int OrganizationId { get; set; }
        public DateTimeOffset DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
        //relationships
        [InverseProperty("EducationalHistory")]
        public virtual Person PersonOfEducation { get; set; }
        [InverseProperty("ProfessionalHistory")]
        public virtual Person PersonOfProfession { get; set; }

        public History History { get; set; }
    }
}
