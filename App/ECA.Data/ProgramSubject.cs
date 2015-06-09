using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    public class ProgramSubject
    {
        private const int PROGRAM_SUBJECT_CODE_LENGTH = 7;

        public ProgramSubject()
        {
            this.History = new History();
        }


        public int ProgramSubjectId { get; set; }
        [MinLength(PROGRAM_SUBJECT_CODE_LENGTH), MaxLength(PROGRAM_SUBJECT_CODE_LENGTH)]
        public string ProgramSubjectCode { get; set; }
         public string Description { get; set; }
        public History History { get; set; }
    }
}
