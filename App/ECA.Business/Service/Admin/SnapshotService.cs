using ECA.Core.Service;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Business.Queries.Admin;
using System.Collections.Generic;
using ECA.Business.Queries.Models.Admin;
using System;
using System.Linq;
using ECA.Business.Service.Programs;

namespace ECA.Business.Service.Admin
{
    public class SnapshotService : DbContextService<EcaContext>, ISnapshotService
    {
        public SnapshotService(EcaContext context, IProgramService programService, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            this.programService = programService;
        }

        private IProgramService programService;

        #region Snapshot Counts

        /// <summary>
        /// Combine all count values into one object
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotCountModelDTO GetProgramCounts(int programId)
        {
            var programs = programService.GetAllChildProgramsWithParent(programId).Where(x => x.ProgramStatusId == ProgramStatus.Active.Id);
            var childPrograms = programs.Select(p => p.ProgramId).ToList();

            SnapshotCountModelDTO snapshotModel = new SnapshotCountModelDTO
            {
                ProgramRelatedProjectsCount = GetProgramRelatedProjectsCount(childPrograms),
                ProgramParticipantCount = GetProgramParticipantCount(childPrograms),
                ProgramBudgetTotal = GetProgramBudgetTotal(childPrograms),
                ProgramFundingSourcesCount = GetProgramFundingSourcesCount(programId),
                ProgramCountryCountAsync = GetProgramCountryCountAsync(childPrograms),
                ProgramBeneficiaryCount = GetProgramBeneficiaryCount(childPrograms),
                ProgramImpactStoryCount = GetProgramImpactStoryCount(childPrograms),
                ProgramAlumniCount = GetProgramAlumniCount(childPrograms),
                ProgramProminenceCount = GetProgramProminenceCount(childPrograms)
            };

            return snapshotModel;
        }
        
        /// <summary>
        /// Count of related projects
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramRelatedProjectsCount(List<int> programIds)
        {   
            var dto = SnapshotQueries.CreateGetProgramRelatedProjectsCountQuery(Context, programIds);
            return dto;
        }

        /// <summary>
        /// Count of participants
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramParticipantCount(List<int> programIds)
        {
            var dto = SnapshotQueries.CreateGetProgramParticipantsCountQuery(Context, programIds);
            return dto;
        }

        /// <summary>
        /// Total budget
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramBudgetTotal(List<int> programIds)
        {
            var dto = SnapshotQueries.CreateGetProgramBudgetTotalQuery(Context, programIds);
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
        public SnapshotDTO GetProgramCountryCountAsync(List<int> programIds)
        {
            var query = SnapshotQueries.CreateGetProgramCountriesByProgramIdsQuery(Context, programIds);
            var locationIds = query.Select(x => x).Count();

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
        public SnapshotDTO GetProgramBeneficiaryCount(List<int> programIds)
        {
            var dto = SnapshotQueries.CreateGetProgramBeneficiaryCountQuery(Context, programIds);
            return dto;
        }

        /// <summary>
        /// Count of impact stories
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramImpactStoryCount(List<int> programIds)
        {
            var query = SnapshotQueries.CreateGetProgramImpactStoryCountQuery(Context, programIds);
            var impactIds = query.Select(x => x).Distinct().Count();

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
        public SnapshotDTO GetProgramAlumniCount(List<int> programIds)
        {
            var dto = SnapshotQueries.CreateGetProgramAlumniCountQuery(Context, programIds);
            return dto;
        }

        /// <summary>
        /// Count of prominence
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramProminenceCount(List<int> programIds)
        {
            var query = SnapshotQueries.CreateGetProgramProminenceCountQuery(Context, programIds);
            var catIds = query.Select(x => x).Distinct().Count();
            
            return new SnapshotDTO()
            {
                DataLabel = "PROMINENCE",
                DataValue = catIds
            };
        }

        #endregion

        #region Snapshot graphs

        /// <summary>
        /// Budget total by year
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public async Task<SnapshotGraphDTO> GetProgramBudgetByYear(int programId)
        {
            var childPrograms = programService.GetAllChildProgramsWithParent(programId);
            var dto = await SnapshotQueries.CreateGetProgramBudgetByYearQuery(Context, childPrograms.Select(p => p.ProgramId).ToList());

            SnapshotGraphDTO graphValues = new SnapshotGraphDTO
            {
                key = "Budget",
                values = dto.OrderBy(x => x.Key).ToList()
            };

            return graphValues;
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
            var childPrograms = programService.GetAllChildProgramsWithParent(programId);
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
            var childPrograms = programService.GetAllChildProgramsWithParent(programId);
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
            var childPrograms = programService.GetAllChildProgramsWithParent(programId);
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

        #endregion

    }
}
