using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class UpdateStudentValidator : AbstractValidator<UpdateStudent>
    {
        public UpdateStudentValidator()
        {
            RuleFor(o => o.student).NotNull().WithMessage("Student information is required");

        }
    }
}