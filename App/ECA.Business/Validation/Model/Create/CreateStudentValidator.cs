using ECA.Business.Validation.Model.Shared;
using FluentValidation;

namespace ECA.Business.Validation.Model.Create
{
    public class CreateStudentValidator : AbstractValidator<CreateStudent>
    {
        public CreateStudentValidator()
        {
            RuleFor(createstudent => createstudent.student).NotNull().WithMessage("Student is required").SetValidator(new StudentValidator());
        }

    }
}
