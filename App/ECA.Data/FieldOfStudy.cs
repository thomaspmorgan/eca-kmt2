using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    public class FieldOfStudy
    {
        private const int FIELD_OF_STUDY_CODE_LENGTH = 7;

        public FieldOfStudy()
        {
            this.History = new History();
        }

        public int FieldOfStudyId { get; set; }
        [MinLength(FIELD_OF_STUDY_CODE_LENGTH), MaxLength(FIELD_OF_STUDY_CODE_LENGTH)]
        public string FieldOfStudyCode { get; set; }
        public string Description { get; set; }
        public History History { get; set; }
    }
}
