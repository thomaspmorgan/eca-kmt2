using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class EducationalInfoValidator : AbstractValidator<EducationalInfo>
    {
        public const int MAJOR_MAX_LENGTH = 7;
        public const int STUDY_MAX_LENGTH = 2;
        public const int REMARKS_MAX_LENGTH = 500;

        public EducationalInfoValidator()
        {
            RuleFor(student => student.eduLevel).NotNull().SetValidator(new EduLevelValidator());
            RuleFor(student => student.PrimaryMajor).NotNull().WithMessage("Education: Primary Major is required").Length(1, MAJOR_MAX_LENGTH).WithMessage("Education: Primary Major can be up to " + MAJOR_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.SecondMajor).Length(0, MAJOR_MAX_LENGTH).WithMessage("Education: Secondary Major can be up to " + MAJOR_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Minor).Length(0, MAJOR_MAX_LENGTH).WithMessage("Education: Minor can be up to " + MAJOR_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.LengthOfStudy).NotNull().WithMessage("Education: Length of Study is required").Length(1, STUDY_MAX_LENGTH).WithMessage("Education: Length of Study can be up to " + STUDY_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.PrgStartDate).NotNull().WithMessage("Education: Start Date is required");
            RuleFor(student => student.PrgEndDate).NotNull().WithMessage("Education: End Date is required");
            RuleFor(student => student.engProficiency).NotNull().WithMessage("Education: English Proficiency is required").SetValidator(new EngProficiencyValidator());
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Education: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }

    }
}