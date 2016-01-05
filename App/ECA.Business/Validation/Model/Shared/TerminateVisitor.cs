using System;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class TerminateVisitor
    {
        public TerminateVisitor()
        { }

        public string Reason { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string OtherRemarks { get; set; }

        public string Remarks { get; set; }
    }
}