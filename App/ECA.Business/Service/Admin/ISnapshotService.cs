using System.Collections.Generic;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Service.Admin
{
    public interface ISnapshotService
    {
        Task<SnapshotDTO> GetProgramCountryCount(int programId);

        SnapshotDTO GetProgramRelatedProjectsCount(int programId);

        SnapshotDTO GetProgramParticipantCount(int programId);

        SnapshotDTO GetProgramFundingSourcesCount(int programId);

        SnapshotDTO GetProgramAlumniCount(int programId);

        SnapshotDTO GetProgramBudgetTotal(int programId);

        SnapshotDTO GetProgramImpactStoryCount(int programId);

        SnapshotDTO GetProgramBeneficiaryCount(int programId);

        SnapshotDTO GetProgramProminenceCount(int programId);

        Task<IEnumerable<SnapshotDTO>> GetProgramBudgetByYear(int programId);

        Task<IEnumerable<SnapshotDTO>> GetProgramMostFundedCountries(int programId);

        Task<IEnumerable<string>> GetProgramTopThemes(int programId);

        Task<IEnumerable<SnapshotDTO>> GetProgramParticipantLocations(int programId);

        Task<IEnumerable<SnapshotDTO>> GetProgramParticipantsByYear(int programId);

        Task<IEnumerable<SnapshotDTO>> GetProgramParticipantGender(int programId);

        Task<IEnumerable<SnapshotDTO>> GetProgramParticipantAge(int programId);

        Task<IEnumerable<SnapshotDTO>> GetProgramParticipantEducation(int programId);
        
    }
}
