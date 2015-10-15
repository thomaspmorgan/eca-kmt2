using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    public class EducationLevel : IHistorical
    {
        private const int DESCRIPTION_CODE_LENGTH = 100;
        private const int EDUCATION_LEVEL_CODE_LENGTH = 2;

        public EducationLevel()
        {
            this.History = new History();
        }

        [Key]
        public int EducationLevelId { get; set; }

        [MinLength(DESCRIPTION_CODE_LENGTH), MaxLength(DESCRIPTION_CODE_LENGTH)]
        public char EducationLevelCode { get; set; }

        [MaxLength(DESCRIPTION_CODE_LENGTH)]
        public string Description { get; set; }

        public char F_1_Ind { get; set; }

        public char M_1_Ind { get; set; }

        public History History { get; set; }
    }
}
