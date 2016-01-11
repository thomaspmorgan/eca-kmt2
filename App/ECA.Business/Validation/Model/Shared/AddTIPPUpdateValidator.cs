using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class AddTIPPUpdateValidator : AbstractValidator<AddTIPPUpdate>
    {
        public AddTIPPUpdateValidator()
        {
            RuleFor(tipp => tipp.print7002).NotNull().WithMessage("T/IPP information: Print request indicator is required");
            RuleFor(tipp => tipp.ParticipantInfo).SetValidator(new ParticipantInfoUpdateValidator()).When(tipp => tipp.ParticipantInfo != null);
            RuleFor(tipp => tipp.TippSite).SetValidator(new TippSiteValidator()).When(tipp => tipp.TippSite != null);
        }
    }
}