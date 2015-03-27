using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.Logging;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class ProjectStatusService : LookupService<ProjectStatusDTO>//DbContextService<EcaContext>
    {
        private static readonly string COMPONENT_NAME = typeof(ProjectService).FullName;
        private readonly ILogger logger;

        public ProjectStatusService(EcaContext context, ILogger logger)
            : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
            this.logger = logger;
        }

        #region Get
        protected override IQueryable<ProjectStatusDTO> GetSelectDTOQuery()
        {
            var query = this.Context.ProjectStatuses.Select(x => new ProjectStatusDTO
            {
                Id = x.ProjectStatusId,
                Name = x.Status
            });
            return query;
        }
        #endregion
    }
}
