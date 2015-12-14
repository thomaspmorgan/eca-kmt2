using ECA.Business.Validation.Model.Shared;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class StudentRegistrationValidator : AbstractValidator<StudentRegistration>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public StudentRegistrationValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Personal Info: Print form option is required");
            When(student => student.usAddress != null, () =>
            {
                RuleFor(student => student.usAddress).SetValidator(new USAddressValidator());
            });
            When(student => student.foreignAddress != null, () =>
            {
                RuleFor(student => student.foreignAddress).SetValidator(new ForeignAddressValidator());
            });
            When(student => student.travelInfo != null, () =>
            {
                RuleFor(student => student.travelInfo).SetValidator(new TravelInfoValidator());
            });
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Personal Info: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}