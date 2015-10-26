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
