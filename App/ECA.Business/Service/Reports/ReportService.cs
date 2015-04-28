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
        public IQueryable<ProjectAwardDTO> GetProjectAwards(int programId, int countryId)
        {
            logger.Trace("Getting Project Awards for Program: [{0}], CountryId [{1}]", programId, countryId);
            return this.GetProjectAwardDTOQuery(programId, countryId);
        }

        #endregion

        #region Queries
        protected IQueryable<ProjectAwardDTO> GetProjectAwardDTOQuery(int programId, int countryId)
        {
            return ReportQueries.CreateGetProjectAward(this.Context, programId, countryId);
        }

        public string GetProgramName(int programId)
        {
            return ReportQueries.CreateGetProgramName(this.Context, programId);
        }

        public string GetCountryName(int countryId)
        {
            return ReportQueries.CreateGetCountryName(this.Context, countryId);
        }
        #endregion
    }
}
