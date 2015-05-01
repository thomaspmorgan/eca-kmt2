using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class MaritalStatusService : LookupService<SimpleLookupDTO>, IMaritalStatusService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public MaritalStatusService(EcaContext context) 
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        protected override IQueryable<SimpleLookupDTO> GetSelectDTOQuery()
        {
            var query = this.Context.MaritalStatuses.Select(x => new SimpleLookupDTO
            {
                Id = x.MaritalStatusId,
                Value = x.Description
            });
            return query;
        }
    }
}
