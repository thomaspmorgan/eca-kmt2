﻿using ECA.Core.Data;
using System;

namespace ECA.Business.Queries.Models.Programs
{
    public class ProgramSnapshotDTO : IConcurrent
    {

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
        
        public byte[] RowVersion { get; set; }
    }
}