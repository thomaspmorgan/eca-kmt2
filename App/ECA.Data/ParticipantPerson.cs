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
        private const int FIELD_OF_STUDY_CODE_LENGTH = 7;
        private const int PROGRAM_SUBJECT_CODE_LENGTH = 7;
        private const int POSITION_CODE_LENGTH = 3;

        public ParticipantPerson()
        {
            this.History = new History();
        }

        [Key]
        public int ParticipantId { get; set; }
        [MaxLength(SEVIS_ID_MAX_LENGTH)]
        public string SevisId { get; set; }
        /// <summary>
        /// Can the participant be contacted? (agreement to contact is in place)
        /// </summary>
        public bool ContactAgreement { get; set; }
        [MaxLength(STUDY_PROJECT_MAX_LENGTH)]
        public string StudyProject {get; set;}
        [MinLength(FIELD_OF_STUDY_CODE_LENGTH), MaxLength(FIELD_OF_STUDY_CODE_LENGTH)]
        public string FieldOfStudyCode {get; set;}
        [MinLength(PROGRAM_SUBJECT_CODE_LENGTH), MaxLength(PROGRAM_SUBJECT_CODE_LENGTH)]
        public string ProgramSubjectCode {get; set;}
        [MinLength(POSITION_CODE_LENGTH), MaxLength(POSITION_CODE_LENGTH)]
        public string PositionCode {get; set;}

        //Relationships
        public Organization HostInstitution {get; set;}
        public int HostInstitutionId {get; set;}
        public Organization HomeInstitution {get; set;}
        public int HomeInstitutionId {get; set;}

        public History History { get; set; }
    }
}
