using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(OPTEmploymentValidator))]
    public class OPTEmployment
    {
        public AddOPTEmployment addOPTEmployment { get; set; }

        public CancelOPTEmployment cancelOPTEmployment { get; set; }

        public EditOPTEmployment editOPTEmployment { get; set; }

        public ExtendOPTEmployment extendOPTEmployment { get; set; }

        public OPTReportParticipation otpReportParticipation { get; set; }
    }
}
