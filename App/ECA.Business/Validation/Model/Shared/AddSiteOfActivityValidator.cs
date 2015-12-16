using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class AddSiteOfActivityValidator : AbstractValidator<AddSiteOfActivity>
    {
        public AddSiteOfActivityValidator()
        {
            RuleFor(student => student.siteOfActivity).NotNull().WithMessage("Visitor: Site of activity address is required").SetValidator(new SiteOfActivityValidator()).When(student => student.siteOfActivity != null);
        }
    }
}