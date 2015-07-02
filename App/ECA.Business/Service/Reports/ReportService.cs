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
using System.Data.Entity;
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
        public async Task<List<ProjectAwardDTO>> GetProjectAwardsAsync(int programId, int countryId)
        {
            logger.Trace("Getting Project Awards (Async) for Program: [{0}], CountryId [{1}]", programId, countryId);
            return await ReportQueries.CreateGetProjectAward(this.Context, programId, countryId).ToListAsync();
        }

        public IQueryable<ProjectAwardDTO> GetProjectAwards(int programId, int countryId)
        {
            logger.Trace("Getting Project Awards for Program: [{0}], CountryId [{1}]", programId, countryId);
            return ReportQueries.CreateGetProjectAward(this.Context, programId, countryId);
        }

        public async Task<List<RegionAwardDTO>> GetRegionAwardsAsync(int programId)
        {
            logger.Trace("Getting Region Awards (Async) for program: [{0}]", programId);
            return await ReportQueries.CreateGetRegionAward(this.Context, programId).ToListAsync();
        }

        public IQueryable<RegionAwardDTO> GetRegionAwards(int programId)
        {
            logger.Trace("Getting Region Awards for program: [{0}]", programId);
            return ReportQueries.CreateGetRegionAward(this.Context, programId);
        }

        
        public async Task<List<PostAwardDTO>> GetPostAwardsAsync(int programId)
        {
            logger.Trace("Getting Region Awards (Async) for program: [{0}]", programId);
            return await ReportQueries.CreateGetPostAward(this.Context, programId).ToListAsync();
        }

        public IQueryable<PostAwardDTO> GetPostAwards(int programId)
        {
            logger.Trace("Getting Region Awards for program: [{0}]", programId);
            return ReportQueries.CreateGetPostAward(this.Context, programId);
        }

        public async Task<List<FocusAwardDTO>> GetFocusAwardsAsync(int programId)
        {
            logger.Trace("Getting Focus Awards (Async) for program: [{0}]", programId);
            return await ReportQueries.CreateGetFocusAward(this.Context, programId).ToListAsync();
        }

        public IQueryable<FocusAwardDTO> GetFocusAwards(int programId)
        {
            logger.Trace("Getting Focus Awards for program: [{0}]", programId);
            return ReportQueries.CreateGetFocusAward(this.Context, programId);
        }

        public async Task<List<FocusCategoryAwardDTO>> GetFocusCategoryAwardsAsync(int programId)
        {
            logger.Trace("Getting Focus-Category Awards (Async) for program: [{0}]", programId);
            return await ReportQueries.CreateGetFocusCategoryAward(this.Context, programId).ToListAsync();
        }

        public IQueryable<FocusCategoryAwardDTO> GetFocusCategoryAwards(int programId)
        {
            logger.Trace("Getting Focus-Category Awards for program: [{0}]", programId);
            return ReportQueries.CreateGetFocusCategoryAward(this.Context, programId);
        }
        #endregion

        #region parameters
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
