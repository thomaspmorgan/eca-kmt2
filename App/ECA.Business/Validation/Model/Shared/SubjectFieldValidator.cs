using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class SubjectFieldValidator : AbstractValidator<SubjectField>
    {
        public const int FIELD_CODE_MAX_LENGTH = 7;
        public const int FOREIGN_FIELD_MAX_LENGTH = 100;
        public const int REMARKS_MAX_LENGTH = 500;

        public SubjectFieldValidator()
        {
            RuleFor(student => student.SubjectFieldCode).NotNull().WithMessage("Student: Subject or field of study is required").Length(1, FIELD_CODE_MAX_LENGTH).WithMessage("Student: Subject or field of study can be up to " + FIELD_CODE_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.ForeignDegreeLevel).Length(0, FOREIGN_FIELD_MAX_LENGTH).WithMessage("Student: Foreign Degree Level can be up to " + FOREIGN_FIELD_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.ForeignFieldOfStudy).Length(0, FOREIGN_FIELD_MAX_LENGTH).WithMessage("Student: Foreign Field of Study can be up to " + FOREIGN_FIELD_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Student: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}