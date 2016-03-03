﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.SEVIS.ErrorPaths
{
    public class PermanentResidenceCountryErrorPath : ErrorPath
    {
        public PermanentResidenceCountryErrorPath()
        {
            SetByStaticLookup(SevisErrorType.PermanentResidenceCountry);
        }
    }
}
