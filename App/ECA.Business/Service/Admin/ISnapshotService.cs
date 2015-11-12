using System.Collections.Generic;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Service.Admin
{
    public interface ISnapshotService
    {
        SnapshotCountModelDTO GetProgramCounts(int programId);

        SnapshotDTO GetProgramCountryCountAsync(List<int> programIds);

        SnapshotDTO GetProgramRelatedProjectsCount(List<int> programIds);

        SnapshotDTO GetProgramParticipantCount(List<int> programIds);

        SnapshotDTO GetProgramFundingSourcesCount(int programId);

        SnapshotDTO GetProgramAlumniCount(List<int> programIds);

        SnapshotDTO GetProgramBudgetTotal(List<int> programIds);

        SnapshotDTO GetProgramImpactStoryCount(List<int> programIds);

        SnapshotDTO GetProgramBeneficiaryCount(List<int> programIds);

        SnapshotDTO GetProgramProminenceCount(List<int> programIds);

        Task<SnapshotGraphDTO> GetProgramBudgetByYear(int programId);

        Task<List<SnapshotDTO>> GetProgramMostFundedCountries(int programId);

        Task<IEnumerable<string>> GetProgramTopThemes(int programId);

        Task<IEnumerable<SnapshotDTO>> GetProgramParticipantsByLocation(int programId);

        Task<SnapshotGraphDTO> GetProgramParticipantsByYear(int programId);

        Task<SnapshotGraphDTO> GetProgramParticipantGender(int programId);

        Task<SnapshotGraphDTO> GetProgramParticipantAge(int programId);

        Task<SnapshotGraphDTO> GetProgramParticipantEducation(int programId);
        
    }
}
