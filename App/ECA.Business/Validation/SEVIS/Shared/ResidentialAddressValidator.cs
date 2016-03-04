using ECA.Business.Validation.SEVIS;
using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class ResidentialAddressValidator : AbstractValidator<ResidentialAddress>
    {
        public ResidentialAddressValidator()
        {
            //RuleFor(visitor => visitor.ResidentialType).Length(3).WithMessage("Residential Address: Type of Residential Address code must be 3 characters").WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
        }
    }
}