
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    public class SnapshotCountModelDTO
    {

        public SnapshotDTO ProgramRelatedProjectsCount { get; set; }

        public SnapshotDTO ProgramParticipantCount { get; set; }

        public SnapshotDTO ProgramBudgetTotal { get; set; }

        public SnapshotDTO ProgramFundingSourcesCount { get; set; }

        public Task<SnapshotDTO> ProgramCountryCountAsync { get; set; }

        public SnapshotDTO ProgramBeneficiaryCount { get; set; }

        public Task<SnapshotDTO> ProgramImpactStoryCount { get; set; }

        public SnapshotDTO ProgramAlumniCount { get; set; }

        public Task<SnapshotDTO> ProgramProminenceCount { get; set; }
        
    }
}
