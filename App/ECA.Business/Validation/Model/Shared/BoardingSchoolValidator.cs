using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class BoardingSchoolValidator : AbstractValidator<BoardingSchool>
    {
        public BoardingSchoolValidator()
        {
            RuleFor(student => student.name).Length(0, 60).WithMessage("Boarding School: Name can be up to 60 characters");
            RuleFor(student => student.phone).Length(0, 12).WithMessage("Boarding School: Phone number can be up to 12 characters");
            RuleFor(student => student.phoneExt).Length(0, 4).WithMessage("Boarding School: Phone extension can be up to 4 characters");
        }
    }
}