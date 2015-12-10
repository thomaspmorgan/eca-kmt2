using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(CPTEmploymentValidator))]
    public class CPTEmployment
    {
        public AddCptEmployment addCptEmployment { get; set; }
        
        public CancelCptEmployment cancelCptEmployment { get; set; }
    }
}
