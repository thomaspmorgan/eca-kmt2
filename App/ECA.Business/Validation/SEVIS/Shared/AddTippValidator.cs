using ECA.Business.Validation.Model.CreateEV;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class AddTippValidator : AbstractValidator<AddTIPP>
    {
        public AddTippValidator()
        {
            RuleFor(tipp => tipp.print7002).NotNull().WithMessage("T/IPP information: Print request indicator is required");
            RuleFor(tipp => tipp.ParticipantInfo).SetValidator(new ParticipantInfoValidator()).When(tipp => tipp.ParticipantInfo != null);
            RuleFor(tipp => tipp.TippSite).SetValidator(new TippSiteValidator()).When(tipp => tipp.TippSite != null);
        }
    }
}