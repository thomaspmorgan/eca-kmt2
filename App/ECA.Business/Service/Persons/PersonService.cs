using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.Logging;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ECA.Business.Service.Persons
{
    public class PersonService : DbContextService<EcaContext>, IPersonService
    {
        public PersonService(EcaContext context, ILogger logger) : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
        }

        public PiiDTO GetPiiById(int personId)
        {
            return PersonQueries.CreateGetPiiByIdQuery(this.Context, personId).SingleOrDefault();
        }

        public Task<PiiDTO> GetPiiByIdAsync(int personId)
        {
            return PersonQueries.CreateGetPiiByIdQuery(this.Context, personId).SingleOrDefaultAsync();
        }
    }
}
