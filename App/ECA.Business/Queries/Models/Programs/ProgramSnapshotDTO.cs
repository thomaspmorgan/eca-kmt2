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
            this.Themes = new List<SimpleLookupDTO>();
            this.FiscalBudget = new List<MoneyFlowDTO>();



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

        public IEnumerable<SimpleLookupDTO> Themes { get; set; }

        public IEnumerable<MoneyFlowDTO> FiscalBudget { get; set; }





        public byte[] RowVersion { get; set; }
    }
}
