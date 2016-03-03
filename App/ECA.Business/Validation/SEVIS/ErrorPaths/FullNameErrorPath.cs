using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.SEVIS.ErrorPaths
{
    public class FullNameErrorPath : ErrorPath
    {
        public FullNameErrorPath()
        {
            SetByStaticLookup(SevisErrorType.FullName);
        }
    }
}
