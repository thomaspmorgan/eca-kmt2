using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class CPTEmploymentValidator : AbstractValidator<CPTEmployment>
    {
        public CPTEmploymentValidator()
        {
            RuleFor(student => student.addCptEmployment).SetValidator(new AddCptEmploymentValidator());
            RuleFor(student => student.cancelCptEmployment).SetValidator(new CancelCptEmploymentValidator());
        }
    }
}