using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace ECA.Data
{

    public class ProfessionEducation : IHistorical
    {
        private const int MAX_NAME_LENGTH = 120;

        [Key]
        public int ProfessionEducationId { get; set; }

        [Required]
        [MaxLength(MAX_NAME_LENGTH)]
        public string Title { get; set; }

        public string Role { get; set; }

        //public Organization Organization { get; set; }
        public int? OrganizationId { get; set; }

        public DateTimeOffset DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
        
        [ForeignKey("PersonOfEducation")]
        public int? PersonOfEducation_PersonId { get; set; }
        [ForeignKey("PersonOfProfession")]
        public int? PersonOfProfession_PersonId { get; set; }
        
        //relationships
        [InverseProperty("EducationalHistory")]
        public Person PersonOfEducation { get; set; }
        [InverseProperty("ProfessionalHistory")]
        public Person PersonOfProfession { get; set; }
        
        public History History { get; set; }
    }
}
