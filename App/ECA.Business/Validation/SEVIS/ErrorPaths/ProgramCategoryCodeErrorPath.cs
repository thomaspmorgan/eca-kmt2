using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.SEVIS
{
    public class ProgramCategoryCodeErrorPath : ErrorPath
    {
        public ProgramCategoryCodeErrorPath()
        {
            SetByStaticLookup(SevisErrorType.ProgramCategoryCode);
        }
    }
}
