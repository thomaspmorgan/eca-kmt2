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
    public class ParticipantPerson
    {

        /// <summary>
        /// Gets the max length of the SEVIS Id.
        /// </summary>
        private const int SEVIS_ID_MAX_LENGTH = 11;
        private const int STUDY_PROJECT_MAX_LENGTH = 250;

        public ParticipantPerson()
        {
            this.History = new History();
        }

        [Key]
        public int ParticipantId { get; set; }
        [MaxLength(SEVIS_ID_MAX_LENGTH)]
        public string SevisId { get; set; }

        [MaxLength(STUDY_PROJECT_MAX_LENGTH)]
        public string StudyProject {get; set;}

        // Relationships
        public int? FieldOfStudyId { get; set; }
        public FieldOfStudy FieldOfStudy { get; set; }

        public int? ProgramSubjectId { get; set; }
        public ProgramSubject ProgramSubject { get; set; }

        public int? PositionId { get; set; }
        public Position Position { get; set; }

        public Organization HostInstitution {get; set;}
        public int? HostInstitutionId {get; set;}

        public Organization HomeInstitution {get; set;}
        public int? HomeInstitutionId {get; set;}

        public Participant Participant { get; set; }

        public History History { get; set; }
    }
}
