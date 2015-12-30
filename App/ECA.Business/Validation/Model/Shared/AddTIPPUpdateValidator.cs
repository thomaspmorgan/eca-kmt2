using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class AddTIPPUpdateValidator : AbstractValidator<AddTIPPUpdate>
    {
        public AddTIPPUpdateValidator()
        {
            RuleFor(student => student.print7002).NotNull().WithMessage("T/IPP information: Print request indicator is required");
            RuleFor(student => student.ParticipantInfo).SetValidator(new ParticipantInfoUpdateValidator()).When(student => student.ParticipantInfo != null);
            RuleFor(student => student.TippSite).SetValidator(new TippSiteValidator()).When(student => student.TippSite != null);
        }
    }
}