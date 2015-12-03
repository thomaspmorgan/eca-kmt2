using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class PersonalInfoValidator : AbstractValidator<PersonalInfo>
    {
        public PersonalInfoValidator()
        {
            RuleFor(student => student.fullName).NotNull().WithMessage("Full Name is required");




        }
    }
}
