using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class BoardingSchoolValidator : AbstractValidator<BoardingSchool>
    {
        public const int NAME_MAX_LENGTH = 50;
        public const int PHONE_MAX_LENGTH = 12;
        public const int PHONEXT_MAX_LENGTH = 4;

        public BoardingSchoolValidator()
        {
            RuleFor(student => student.Name).Length(0, NAME_MAX_LENGTH).WithMessage("Boarding School: Name can be up to " + NAME_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Phone).Length(0, PHONE_MAX_LENGTH).WithMessage("Boarding School: Phone number can be up to " + PHONE_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.PhoneExt).Length(0, PHONEXT_MAX_LENGTH).WithMessage("Boarding School: Phone extension can be up to " + PHONEXT_MAX_LENGTH.ToString() + " characters");
        }
    }
}