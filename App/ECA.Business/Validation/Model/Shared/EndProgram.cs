using System;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class EndProgram
    {
        public EndProgram()
        { }

        public string Reason { get; set; }

        public DateTime EffectiveDate { get; set; }
        
        public string Remarks { get; set; }
    }
}