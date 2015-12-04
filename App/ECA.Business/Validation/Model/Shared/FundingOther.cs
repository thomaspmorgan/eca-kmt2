using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(FundingOtherValidator))]
    public class FundingOther
    {
        public int Amount { get; set; }
        
        public string Remarks { get; set; }
    }
}
