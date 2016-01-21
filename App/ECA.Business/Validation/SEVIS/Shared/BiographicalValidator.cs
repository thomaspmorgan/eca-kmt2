using ECA.Business.Validation.Model.Shared;
using ECA.Business.Validation.SEVIS;
using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class BiographicalValidator : AbstractValidator<Biographical>
    {
        public const int GENDER_CODE_LENGTH = 1;
        public const int CITY_MAX_LENGTH = 50;
        public const int COUNTRY_CODE_LENGTH = 2;
        public const int VISA_TYPE_LENGTH = 2;
        public const int BIRTH_COUNTRY_REASON_LENGTH = 2;
        public const int EMAIL_MAX_LENGTH = 255;

        public BiographicalValidator()
        {
            RuleFor(visitor => visitor.FullName).NotNull().WithMessage("EV Biographical Info: Full Name is required").SetValidator(new FullNameValidator()).When(visitor => visitor.FullName != null);
            RuleFor(visitor => visitor.BirthDate).NotNull().WithMessage("EV Biographical Info: Date of Birth is required").WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.Gender).NotNull().WithMessage("EV Biographical Info: Gender is required").Length(GENDER_CODE_LENGTH).WithMessage("EV Biographical Info: Gender must be " + GENDER_CODE_LENGTH.ToString() + " character").WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.BirthCity).Length(1, CITY_MAX_LENGTH).WithMessage("EV Biographical Info: City of Birth is required and can be up to " + CITY_MAX_LENGTH.ToString() + " characters").WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.BirthCountryCode).Length(COUNTRY_CODE_LENGTH).WithMessage("EV Biographical Info: Country of Birth is required and must be " + COUNTRY_CODE_LENGTH.ToString() + " characters").WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.CitizenshipCountryCode).Length(COUNTRY_CODE_LENGTH).WithMessage("EV Biographical Info: Country of Citizenship is required and must be " + COUNTRY_CODE_LENGTH.ToString() + " characters").WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.PermanentResidenceCountryCode).Length(COUNTRY_CODE_LENGTH).WithMessage("EV Biographical Info: Permanent Residence Country is required and must be " + COUNTRY_CODE_LENGTH.ToString() + " characters").WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.BirthCountryReason).Length(0, BIRTH_COUNTRY_REASON_LENGTH).WithMessage("EV Biographical Info: Birth Country Reason must be " + BIRTH_COUNTRY_REASON_LENGTH.ToString() + " characters").WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.EmailAddress).Length(0, EMAIL_MAX_LENGTH).WithMessage("EV Biographical Info: Email can be up to " + EMAIL_MAX_LENGTH.ToString() + " characters").EmailAddress().WithMessage("EV Biographical Info: Email is invalid").WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.Contact.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
        }
    }
}