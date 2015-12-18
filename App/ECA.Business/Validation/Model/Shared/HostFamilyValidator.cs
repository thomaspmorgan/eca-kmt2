using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class HostFamilyValidator : AbstractValidator<HostFamily>
    {
        public HostFamilyValidator()
        {
            RuleFor(student => student.phone).Length(0, 12).WithMessage("Host Family: Phone number can be up to 12 characters");
            RuleFor(student => student.phoneExt).Length(0, 4).WithMessage("Host Family: Phone extension can be up to 4 characters");
        }
    }
}