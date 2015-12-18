using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class ResidentialAddressValidator : AbstractValidator<ResidentialAddress>
    {
        public ResidentialAddressValidator()
        {
            RuleFor(student => student.ResidentialType).Length(3).WithMessage("Residential Address: Type of Residential Address code must be 3 characters");
        }
    }
}