using ECA.Business.Validation.SEVIS;
using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class HostFamilyValidator : AbstractValidator<HostFamily>
    {
        public const int PHONE_MAX_LENGTH = 12;
        public const int PHONEXT_MAX_LENGTH = 4;

        public HostFamilyValidator()
        {
            RuleFor(student => student.Phone).Length(0, PHONE_MAX_LENGTH).WithMessage("Host Family: Phone number can be up to " + PHONE_MAX_LENGTH.ToString() + " characters");
            //.WithState(x => new ErrorPath { Category = ElementCategory.Project.ToString(), CategorySub = ElementCategorySub.Participant.ToString(), Tab = ElementCategorySectionTab.Sevis.ToString() });
            RuleFor(student => student.PhoneExt).Length(0, PHONEXT_MAX_LENGTH).WithMessage("Host Family: Phone extension can be up to " + PHONEXT_MAX_LENGTH.ToString() + " characters");
                //.WithState(x => new ErrorPath { Category = ElementCategory.Project.ToString(), CategorySub = ElementCategorySub.Participant.ToString(), Tab = ElementCategorySectionTab.Sevis.ToString() });
        }
    }
}