using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class OffCampusEmploymentValidator : AbstractValidator<OffCampusEmployment>
    {
        public OffCampusEmploymentValidator()
        {
            When(student => student.addOCEmployment != null, () =>
            {
                RuleFor(student => student.addOCEmployment).SetValidator(new AddOCEmploymentValidator());
            });
            When(student => student.cancelOCEmployment != null, () =>
            {
                RuleFor(student => student.cancelOCEmployment).SetValidator(new CancelOCEmploymentValidator());
            });
            When(student => student.editOCEmployment != null, () =>
            {
                RuleFor(student => student.editOCEmployment).SetValidator(new EditOCEmploymentValidator());
            });
        }
    }
}