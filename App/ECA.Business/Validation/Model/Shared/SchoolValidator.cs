using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class SchoolValidator : AbstractValidator<School>
    {
        public const int REMARKS_MAX_LENGTH = 500;

        public SchoolValidator()
        {
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Funding: School Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}