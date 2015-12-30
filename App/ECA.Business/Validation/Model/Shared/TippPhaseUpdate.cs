using System;

namespace ECA.Business.Validation.Model.Shared
{
    public class TippPhaseUpdate
    {
        public string PhaseId { get; set; }
        
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}