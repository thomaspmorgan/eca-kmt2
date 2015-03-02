using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    public abstract class LookupService
    {
        private EcaContext context;

        public LookupService(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }
    }
}
