using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public class PersonService
    {
        private EcaContext context;

        public PersonService(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            this.context = context;
        }
    }
}
