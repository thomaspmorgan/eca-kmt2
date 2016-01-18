using System;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(TippPhaseTippPhaseUpdateValidator))]
    public class TippPhaseUpdate
    {
        public TippPhaseUpdate()
        { }

        public string PhaseId { get; set; }
        
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}