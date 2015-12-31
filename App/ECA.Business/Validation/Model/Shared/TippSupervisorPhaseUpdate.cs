using System;

namespace ECA.Business.Validation.Model.Shared
{
    public class TippSupervisorPhaseUpdate
    {
        public TippSupervisorPhaseUpdate()
        { }

        public string PhaseId { get; set; }

        public DateTime SignatureDate { get; set; }
    }
}