using ECA.Core.Service;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Business.Queries.Admin;
using System.Collections.Generic;
using ECA.Business.Queries.Models.Admin;
using System;

namespace ECA.Business.Service.Admin
{
    public class SnapshotService : DbContextService<EcaContext>, ISnapshotService
    {
        public SnapshotService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
        }

        public SnapshotDTO GetProgramRelatedProjectsCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramRelatedProjectsCountQuery(Context, programId);
            return dto;
        }

        public SnapshotDTO GetProgramParticipantCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramParticipantsCountQuery(Context, programId);
            return dto;
        }

        public SnapshotDTO GetProgramBudgetTotal(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramBudgetTotalQuery(Context, programId);
            return dto;
        }

        public SnapshotDTO GetProgramFundingSourcesCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramFundingSourceCountQuery(Context, programId);
            return dto;
        }

        /// <summary>
        /// Get number of countries associated with program
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public SnapshotDTO GetProgramCountryCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramCountryCountQuery(Context, programId);
            return dto;
        }

        public SnapshotDTO GetProgramBeneficiaryCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramBeneficiaryCountQuery(Context, programId);
            return dto;
        }

        public SnapshotDTO GetProgramImpactStoryCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramImpactStoryCountQuery(Context, programId);
            return dto;
        }

        public SnapshotDTO GetProgramAlumniCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramAlumniCountQuery(Context, programId);
            return dto;
        }

        public SnapshotDTO GetProgramProminenceCount(int programId)
        {
            var dto = SnapshotQueries.CreateGetProgramProminenceCountQuery(Context, programId);
            return dto;
        }

        public async Task<IEnumerable<SnapshotDTO>> GetProgramBudgetByYear(int programId)
        {
            var dto = await SnapshotQueries.CreateGetProgramBudgetByYearQuery(Context, programId);
            return dto;
        }

        public Task<IEnumerable<SnapshotDTO>> GetProgramMostFundedCountries(int programId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetProgramTopThemes(int programId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SnapshotDTO>> GetProgramParticipantLocations(int programId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SnapshotDTO>> GetProgramParticipantsByYear(int programId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SnapshotDTO>> GetProgramParticipantGender(int programId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SnapshotDTO>> GetProgramParticipantAge(int programId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SnapshotDTO>> GetProgramParticipantEducation(int programId)
        {
            throw new NotImplementedException();
        }
        
    }
}
