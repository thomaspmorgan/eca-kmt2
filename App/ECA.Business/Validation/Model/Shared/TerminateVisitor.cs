using System;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(TerminateVisitorValidator))]
    public class TerminateVisitor
    {
        public TerminateVisitor()
        { }

        /// <summary>
        /// Termination reason code
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Termination date
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        public string OtherRemarks { get; set; }

        public string Remarks { get; set; }
    }
}