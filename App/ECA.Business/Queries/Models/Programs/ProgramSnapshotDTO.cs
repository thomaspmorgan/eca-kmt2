using ECA.Core.Data;
using System;
using System.Collections.Generic;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;

namespace ECA.Business.Queries.Models.Programs
{
    public class ProgramSnapshotDTO : IConcurrent
    {
        public ProgramSnapshotDTO()
        {
            this.BudgetByYear = new List<SnapshotDTO>();
            this.MostFundedCountries = new List<SnapshotDTO>();
            this.TopThemes = new List<string>();
            this.ParticipantLocations = new List<SnapshotDTO>();
            this.ParticipantsByYear = new List<SnapshotDTO>();
            this.ParticipantGenders = new List<SnapshotDTO>();
            this.ParticipantAges = new List<SnapshotDTO>();
            this.ParticipantEducation = new List<SnapshotDTO>();
            this.ParticipantAudienceReach = new List<Tuple<int, int>>();
        }

        public int ProgramId { get; set; }

        public int RelatedProjects { get; set; }

        public int Participants { get; set; }

        public decimal Budget { get; set; }

        public decimal CostPerParticipant { get; set; }

        public int FundingSources { get; set; }

        public int Countries { get; set; }

        public int Beneficiaries { get; set; }

        public int ImpactStories { get; set; }

        public int Alumni { get; set; }

        public int Prominence { get; set; }

        public IEnumerable<SnapshotDTO> BudgetByYear { get; set; }

        public IEnumerable<SnapshotDTO> MostFundedCountries { get; set; }

        public IEnumerable<string> TopThemes { get; set; }

        public IEnumerable<SnapshotDTO> ParticipantLocations { get; set; }

        public IEnumerable<SnapshotDTO> ParticipantsByYear { get; set; }

        public IEnumerable<SnapshotDTO> ParticipantGenders { get; set; }

        public IEnumerable<SnapshotDTO> ParticipantAges { get; set; }

        public IEnumerable<SnapshotDTO> ParticipantEducation { get; set; }

        public IEnumerable<Tuple<int, int>> ParticipantAudienceReach { get; set; }
        
        public byte[] RowVersion { get; set; }
    }
}
