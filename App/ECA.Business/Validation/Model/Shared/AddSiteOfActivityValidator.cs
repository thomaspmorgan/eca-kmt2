using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class AddSiteOfActivityValidator : AbstractValidator<AddSiteOfActivity>
    {
        public AddSiteOfActivityValidator()
        {
            RuleFor(student => student.SiteOfActivitySOA).NotNull().WithMessage("Visitor: Site of activity address is required").SetValidator(new SiteOfActivitySOAValidator()).When(student => student.SiteOfActivitySOA != null);
        }
    }
}