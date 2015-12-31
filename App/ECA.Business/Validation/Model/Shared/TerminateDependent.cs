using System;

namespace ECA.Business.Validation.Model
{
    public class TerminateDependent
    {
        public TerminateDependent()
        { }

        public string dependentSevisID { get; set; }
        
        public string Reason { get; set; }
        
        public DateTime EffectiveDate { get; set; }

        public string OtherRemarks { get; set; }
        
        public string Remarks { get; set; }        
    }
}
