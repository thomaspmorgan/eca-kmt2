using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(FundingValidator))]
    public class Funding
    {
        public int Personal { get; set; }

        public School School { get; set; }

        public FundingOther Other { get; set; }
        
        public int Employment { get; set; }        
    }
}
