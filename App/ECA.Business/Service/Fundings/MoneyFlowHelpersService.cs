using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Fundings
{
    /// <summary>
    /// The ProjectStatusService is used to retrieve project stati from the db context.
    /// </summary>
    public class MoneyFlowStatusService : LookupService<MoneyFlowStatusDTO>, IMoneyFlowStatusService
    {

        /// <summary>
        /// Creates a new ProjectStatusService with the context and logger.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="logger">The logger.</param>
        public MoneyFlowStatusService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get
        /// <summary>
        /// Returns a query to retrieve project status dtos.
        /// </summary>
        /// <returns>A query to get project status dtos.</returns>
        protected override IQueryable<MoneyFlowStatusDTO> GetSelectDTOQuery()
        {
            var query = this.Context.MoneyFlowStatuses.Select(x => new MoneyFlowStatusDTO
            {
                Id = x.MoneyFlowStatusId,
                Name = x.MoneyFlowStatusName
            });
            return query;
        }
        #endregion
    }

    public class MoneyFlowTypeService : LookupService<MoneyFlowTypeDTO>, IMoneyFlowTypeService
    {

        /// <summary>
        /// Creates a new ProjectTypeService with the context and logger.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="logger">The logger.</param>
        public MoneyFlowTypeService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get
        /// <summary>
        /// Returns a query to retrieve project Type dtos.
        /// </summary>
        /// <returns>A query to get project Type dtos.</returns>
        protected override IQueryable<MoneyFlowTypeDTO> GetSelectDTOQuery()
        {
            var query = this.Context.MoneyFlowTypes.Select(x => new MoneyFlowTypeDTO
            {
                Id = x.MoneyFlowTypeId,
                Name = x.MoneyFlowTypeName
            });
            return query;
        }
        #endregion
    }
}
