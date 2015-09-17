using System;
using System.ComponentModel.DataAnnotations;

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
        public Organization Organization { get; set; }
        public int? OrganizationId { get; set; }

        public DateTimeOffset DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
        
        public int? PersonOfEducation_PersonId { get; set; }
        public int? PersonOfProfession_PersonId { get; set; }

        public History History { get; set; }
    }
}
