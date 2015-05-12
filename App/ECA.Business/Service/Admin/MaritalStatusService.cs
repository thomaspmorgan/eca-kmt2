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
    /// <summary>
    /// Service for marital status
    /// </summary>
    public class MaritalStatusService : LookupService<SimpleLookupDTO>, IMaritalStatusService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The context to query</param>
        public MaritalStatusService(EcaContext context) 
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        /// <summary>
        /// Create query
        /// </summary>
        /// <returns>Queryable object of simple lookup dtos</returns>
        protected override IQueryable<SimpleLookupDTO> GetSelectDTOQuery()
        {
            var query = this.Context.MaritalStatuses.Select(x => new SimpleLookupDTO
            {
                Id = x.MaritalStatusId,
                Value = x.Status
            });
            return query;
        }
    }
}
