using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    public class ProgramCategory
    {
        private const int PROGRAM_CATEGORY_CODE_LENGTH = 7;

        public ProgramCategory()
        {
            this.History = new History();
        }

        [Key]
        public int ProgramCategoryId { get; set; }
        [MinLength(PROGRAM_CATEGORY_CODE_LENGTH), MaxLength(PROGRAM_CATEGORY_CODE_LENGTH)]
        public string ProgramCategoryCode { get; set; }
         public string Description { get; set; }
        public History History { get; set; }
    }
}
