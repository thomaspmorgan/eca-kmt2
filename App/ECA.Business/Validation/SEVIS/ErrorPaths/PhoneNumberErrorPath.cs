using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.SEVIS.ErrorPaths
{
    public class PhoneNumberErrorPath : ErrorPath
    {
        public PhoneNumberErrorPath()
        {
            SetByStaticLookup(SevisErrorType.PhoneNumber);
        }
    }
}
