
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    public class SnapshotCountModelDTO
    {
        public SnapshotDTO ProgramRelatedProjectsCount { get; set; }

        public SnapshotDTO ProgramParticipantCount { get; set; }

        public SnapshotDTO ProgramBudgetTotal { get; set; }

        public SnapshotDTO ProgramFundingSourcesCount { get; set; }

        public SnapshotDTO ProgramCountryCountAsync { get; set; }

        public SnapshotDTO ProgramBeneficiaryCount { get; set; }

        public SnapshotDTO ProgramImpactStoryCount { get; set; }

        public SnapshotDTO ProgramAlumniCount { get; set; }

        public SnapshotDTO ProgramProminenceCount { get; set; }        
    }
}
