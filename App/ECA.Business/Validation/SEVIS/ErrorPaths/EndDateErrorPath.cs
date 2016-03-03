using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.SEVIS.ErrorPaths
{
    public class EndDateErrorPath : ErrorPath
    {
        public EndDateErrorPath()
        {
            SetByStaticLookup(SevisErrorType.EndDate);
        }
    }
}
