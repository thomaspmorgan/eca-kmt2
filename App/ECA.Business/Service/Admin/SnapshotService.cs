using ECA.Core.Service;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Business.Queries.Admin;
using System.Collections.Generic;
using ECA.Business.Queries.Models.Admin;
using System;
using System.Linq;

namespace ECA.Business.Service.Admin
{
    public class SnapshotService : DbContextService<EcaContext>, ISnapshotService
    {
        public SnapshotService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
        }

        /// <summary>
        /// Count of related projects
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramRelatedProjectsCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramRelatedProjectsCountQuery(Context, programId);
            return dto;
        }

        /// <summary>
        /// Count of participants
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramParticipantCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramParticipantsCountQuery(Context, programId);
            return dto;
        }

        /// <summary>
        /// Total budget
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramBudgetTotal(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramBudgetTotalQuery(Context, programId);
            return dto;
        }

        /// <summary>
        /// Count of funding sources
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramFundingSourcesCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramFundingSourceCountQuery(Context, programId);
            return dto;
        }

        /// <summary>
        /// Count of countries
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<SnapshotDTO> GetProgramCountryCount(int programId)
        {
            var dto = await SnapshotQueries.CreateGetProgramCountryCountQuery(Context, programId);
            return dto;
        }

        /// <summary>
        /// Count of beneficiaries
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramBeneficiaryCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramBeneficiaryCountQuery(Context, programId);
            return dto;
        }

        /// <summary>
        /// Count of impact stories
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramImpactStoryCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramImpactStoryCountQuery(Context, programId);
            return dto;
        }

        /// <summary>
        /// Count of alumni
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramAlumniCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramAlumniCountQuery(Context, programId);
            return dto;
        }

        /// <summary>
        /// Count of prominence
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramProminenceCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramProminenceCountQuery(Context, programId);
            return dto;
        }

        /// <summary>
        /// Budget total by year
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<SnapshotGraphDTO> GetProgramBudgetByYear(int programId)
        {
            var dto = await SnapshotQueries.CreateGetProgramBudgetByYearQuery(Context, programId);
            return dto;
        }

        /// <summary>
        /// Most funded countries
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public Task<List<SnapshotDTO>> GetProgramMostFundedCountries(int programId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Top themes
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetProgramTopThemes(int programId)
        {
            var dto = await SnapshotQueries.CreateGetProgramTopThemesQuery(Context, programId);
            return dto;
        }

        /// <summary>
        /// Count of participants by top 5 location
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SnapshotDTO>> GetProgramParticipantsByLocation(int programId)
        {
            var dto = await SnapshotQueries.CreateGetProgramParticipantsByLocationQuery(Context, programId);
            return dto;
        }

        /// <summary>
        /// Count of participants by year
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<SnapshotGraphDTO> GetProgramParticipantsByYear(int programId)
        {
            var dto = await SnapshotQueries.CreateGetProgramParticipantsByYearQuery(Context, programId);
            return dto;
        }
        
        /// <summary>
        /// Count of participants by gender
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public Task<SnapshotGraphDTO> GetProgramParticipantGender(int programId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Count of participants by age range
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public Task<SnapshotGraphDTO> GetProgramParticipantAge(int programId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Count of participants by education level
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public Task<SnapshotGraphDTO> GetProgramParticipantEducation(int programId)
        {
            throw new NotImplementedException();
        }
        
    }
}
