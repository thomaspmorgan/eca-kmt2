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
        
        public int? PersonOfEducationId { get; set; }
        public int? PersonOfProfessionId { get; set; }

        public History History { get; set; }
    }
}
