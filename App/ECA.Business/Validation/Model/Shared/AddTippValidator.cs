using ECA.Business.Validation.Model.CreateEV;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class AddTippValidator : AbstractValidator<AddTIPP>
    {
        public AddTippValidator()
        {
            RuleFor(student => student.print7002).NotNull().WithMessage("T/IPP information: Print request indicator is required");
            RuleFor(student => student.ParticipantInfo).SetValidator(new ParticipantInfoValidator()).When(student => student.ParticipantInfo != null);
            RuleFor(student => student.TippSite).SetValidator(new TippSiteValidator()).When(student => student.TippSite != null);
        }
    }
}