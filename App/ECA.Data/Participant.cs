using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ECA.Data
{
    public class Participant
    {
        [Key]
        public int ParticipantId { get; set; }
        public int? OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public int? PersonId { get; set; }
        public Person Person { get; set; }
        [Required]
        public int ParticipantTypeId { get; set; }
        public ParticipantType ParticipantType { get; set; }


        //Relationships
        public ICollection<Project> Projects { get; set; }

        public ICollection<MoneyFlow> SourceParticipantMoneyFlows { get; set; }
        public ICollection<MoneyFlow> RecipientParticipantMoneyFlows { get; set; }

        public History History { get; set; }
    }
}
