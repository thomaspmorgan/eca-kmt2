using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class AddSiteOfActivityValidator : AbstractValidator<AddSiteOfActivity>
    {
        public AddSiteOfActivityValidator()
        {
            RuleFor(soa => soa.SiteOfActivitySOA).NotNull().WithMessage("Visitor: Site of Activity address is required").SetValidator(new SiteOfActivitySOAValidator()).When(soa => soa.SiteOfActivitySOA != null);
            RuleFor(soa => soa.SiteOfActivityExempt).NotNull().WithMessage("Visitor: Site of Activity for Exempt is required").SetValidator(new SiteOfActivityExemptValidator()).When(soa => soa.SiteOfActivityExempt != null);
        }
    }
}