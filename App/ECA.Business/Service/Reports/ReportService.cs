using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using ECA.Business.Queries.Models.Reports;
using NLog;

namespace ECA.Business.Service.Reports
{
    public class ReportService : DbContextService<EcaContext>, IReportService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ReportService(EcaContext context)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null");
        }

        #region Get
        public IQueryable<ProjectAwardDTO> GetProjectAwards(int year, int countryId)
        {
            logger.Trace("Getting Project Awards for Year: [{0}], CountryId [{0}]", year, countryId);
            return this.GetProjectAwardDTOQuery(year, countryId);
        }

        #endregion

        #region Queries
        protected IQueryable<ProjectAwardDTO> GetProjectAwardDTOQuery(int year, int countryId)
        {
            return ReportQueries.CreateGetProjectAward(this.Context, year, countryId);
        }
        #endregion
    }
}
