using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class StudentStatusValidator : AbstractValidator<StudentStatus>
    {
        public StudentStatusValidator()
        {
            When(student => student.cancel != null, () =>
            {
                RuleFor(student => student.cancel).SetValidator(new CancelStudentValidator());
            });
            When(student => student.complete != null, () =>
            {
                RuleFor(student => student.complete).SetValidator(new CompleteStudentValidator());
            });
            When(student => student.terminate != null, () =>
            {
                RuleFor(student => student.terminate).SetValidator(new TerminateStudentValidator());
            });
        }
    }
}