using ECA.Core.Service;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Business.Queries.Admin;
using System.Collections.Generic;
using ECA.Business.Queries.Models.Admin;
using System;
using System.Linq;
using ECA.Business.Service.Programs;
using System.Data.Entity;

namespace ECA.Business.Service.Admin
{
    public class SnapshotService : DbContextService<EcaContext>, ISnapshotService
    {
        public SnapshotService(EcaContext context, IProgramService programService, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            this.programService = programService;
        }

        private IProgramService programService;

        /// <summary>
        /// Count of related projects
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramRelatedProjectsCount(int programId)
        {
            var childPrograms = programService.GetAllChildPrograms(programId);
            var dto = SnapshotQueries.CreateGetProgramRelatedProjectsCountQuery(Context, childPrograms.Select(p => p.ProgramId).ToList());
            return dto;
        }

        /// <summary>
        /// Count of participants
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramParticipantCount(int programId)
        {
            var childPrograms = programService.GetAllChildPrograms(programId);
            var dto = SnapshotQueries.CreateGetProgramParticipantsCountQuery(Context, childPrograms.Select(p => p.ProgramId).ToList());
            return dto;
        }

        /// <summary>
        /// Total budget
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramBudgetTotal(int programId)
        {
            var childPrograms = programService.GetAllChildPrograms(programId);
            var dto = SnapshotQueries.CreateGetProgramBudgetTotalQuery(Context, childPrograms.Select(p => p.ProgramId).ToList());
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
        public async Task<SnapshotDTO> GetProgramCountryCountAsync(int programId)
        {
            var childPrograms = await programService.GetAllChildProgramsAsync(programId);

            var query = SnapshotQueries.CreateGetProgramCountriesByProgramIdsQuery(Context, childPrograms.Select(p => p.ProgramId).ToList());
            var locationIds = await query.Select(x => x).Distinct().CountAsync();
            return new SnapshotDTO()
            {
                DataLabel = "COUNTRIES",
                DataValue = locationIds
            };
        }

        /// <summary>
        /// Count of beneficiaries
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramBeneficiaryCount(int programId)
        {
            var childPrograms = programService.GetAllChildPrograms(programId);
            var dto = SnapshotQueries.CreateGetProgramBeneficiaryCountQuery(Context, childPrograms.Select(p => p.ProgramId).ToList());
            return dto;
        }

        /// <summary>
        /// Count of impact stories
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<SnapshotDTO> GetProgramImpactStoryCount(int programId)
        {
            var childPrograms = programService.GetAllChildPrograms(programId);
            var query = SnapshotQueries.CreateGetProgramImpactStoryCountQuery(Context, childPrograms.Select(p => p.ProgramId).ToList());
            var impactIds = await query.Select(x => x).Distinct().CountAsync();
            return new SnapshotDTO()
            {
                DataLabel = "IMPACT STORIES",
                DataValue = impactIds
            };
        }

        /// <summary>
        /// Count of alumni
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramAlumniCount(int programId)
        {
            var childPrograms = programService.GetAllChildPrograms(programId);
            var dto = SnapshotQueries.CreateGetProgramAlumniCountQuery(Context, childPrograms.Select(p => p.ProgramId).ToList());
            return dto;
        }

        /// <summary>
        /// Count of prominence
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<SnapshotDTO> GetProgramProminenceCount(int programId)
        {
            var childPrograms = programService.GetAllChildPrograms(programId);
            var query = SnapshotQueries.CreateGetProgramProminenceCountQuery(Context, childPrograms.Select(p => p.ProgramId).ToList());
            var catIds = await query.Select(x => x).Distinct().CountAsync();
            
            return new SnapshotDTO()
            {
                DataLabel = "PROMINENCE",
                DataValue = catIds
            };
        }

        /// <summary>
        /// Budget total by year
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<SnapshotGraphDTO> GetProgramBudgetByYear(int programId)
        {
            var childPrograms = programService.GetAllChildPrograms(programId);
            var dto = await SnapshotQueries.CreateGetProgramBudgetByYearQuery(Context, childPrograms.Select(p => p.ProgramId).ToList());
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
            var childPrograms = programService.GetAllChildPrograms(programId);
            var dto = await SnapshotQueries.CreateGetProgramTopThemesQuery(Context, childPrograms.Select(p => p.ProgramId).ToList());
            return dto;
        }

        /// <summary>
        /// Count of participants by top 5 location
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SnapshotDTO>> GetProgramParticipantsByLocation(int programId)
        {
            var childPrograms = programService.GetAllChildPrograms(programId);
            var dto = await SnapshotQueries.CreateGetProgramParticipantsByLocationQuery(Context, childPrograms.Select(p => p.ProgramId).ToList());
            return dto;
        }

        /// <summary>
        /// Count of participants by year
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<SnapshotGraphDTO> GetProgramParticipantsByYear(int programId)
        {
            var childPrograms = programService.GetAllChildPrograms(programId);
            var dto = await SnapshotQueries.CreateGetProgramParticipantsByYearQuery(Context, childPrograms.Select(p => p.ProgramId).ToList());
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
