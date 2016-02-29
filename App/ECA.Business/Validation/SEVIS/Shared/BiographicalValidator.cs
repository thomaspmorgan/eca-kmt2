using ECA.Business.Validation.Model.Shared;
using ECA.Business.Validation.SEVIS;
using ECA.Data;
using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class BiographicalValidator : AbstractValidator<Biographical>
    {
        public const int CITY_MAX_LENGTH = 50;
        public const int COUNTRY_CODE_LENGTH = 2;
        public const int VISA_TYPE_LENGTH = 2;
        public const int BIRTH_COUNTRY_REASON_LENGTH = 2;
        public const int EMAIL_MAX_LENGTH = 255;

        public const string BIRTH_DATE_NULL_ERROR_MESSAGE = "EV Biographical Info: Date of Birth is required";

        public const string FULL_NAME_NULL_ERROR_MESSAGE = "EV Biographical Info: Full Name is required";

        public const string GENDER_REQUIRED_ERROR_MESSAGE = "EV Biographical Info: Gender is required";

        public static string GENDER_MUST_BE_A_VALUE_ERROR_MESSAGE = string.Format("EV Biographical Info: Gender must be {0} or {1}", Gender.SEVIS_MALE_GENDER_CODE_VALUE, Gender.SEVIS_FEMALE_GENDER_CODE_VALUE);

        public static string CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE = string.Format("EV Biographical Info: City of Birth is required and can be up to {0} characters", CITY_MAX_LENGTH);

        public static string BIRTH_COUNTRY_CODE_ERROR_MESSAGE = string.Format("EV Biographical Info: Country of Birth is required and must be {0} characters", COUNTRY_CODE_LENGTH);

        public static string CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE = string.Format("EV Biographical Info: Country of Citizenship is required and must be {0} characters", COUNTRY_CODE_LENGTH);

        public static string PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE = string.Format("EV Biographical Info: Permanent Residence Country is required and must be {0} characters", COUNTRY_CODE_LENGTH);

        public static string BIRTH_COUNTRY_REASON_ERROR_MESSAGE = string.Format("EV Biographical Info: Birth Country Reason must be {0} characters", BIRTH_COUNTRY_REASON_LENGTH);

        public static string EMAIL_ERROR_MESSAGE = string.Format("EV Biographical Info: Email can be up to {0} characters", EMAIL_MAX_LENGTH);

        public static string INVALID_EMAIL_ERROR_MESSAGE = string.Format("EV Biographical Info: Email is invalid");

        public BiographicalValidator()
        {
            RuleFor(visitor => visitor.FullName)
                .NotNull()
                .WithMessage(FULL_NAME_NULL_ERROR_MESSAGE)
                .SetValidator(new FullNameValidator());
            RuleFor(visitor => visitor.BirthDate)
                .NotNull()
                .WithMessage(BIRTH_DATE_NULL_ERROR_MESSAGE)
                .WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.Gender)
                .NotNull()
                .WithMessage(GENDER_REQUIRED_ERROR_MESSAGE)
                .Matches(string.Format("({0}|{1})", Gender.SEVIS_MALE_GENDER_CODE_VALUE, Gender.SEVIS_FEMALE_GENDER_CODE_VALUE))
                .WithMessage(GENDER_MUST_BE_A_VALUE_ERROR_MESSAGE)
                .WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.BirthCity)
                .NotNull()
                .WithMessage(CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE)
                .Length(1, CITY_MAX_LENGTH)
                .WithMessage(CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE)
                .WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.BirthCountryCode)
                .NotNull()
                .WithMessage(BIRTH_COUNTRY_CODE_ERROR_MESSAGE)
                .Length(COUNTRY_CODE_LENGTH)
                .WithMessage(BIRTH_COUNTRY_CODE_ERROR_MESSAGE)
                .WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.CitizenshipCountryCode)
                .NotNull()
                .WithMessage(CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE)
                .Length(COUNTRY_CODE_LENGTH)
                .WithMessage(CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE)
                .WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.PermanentResidenceCountryCode)
                .NotNull()
                .WithMessage(PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE)
                .Length(COUNTRY_CODE_LENGTH)
                .WithMessage(PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE)
                .WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.BirthCountryReason).Length(0, BIRTH_COUNTRY_REASON_LENGTH).WithMessage(BIRTH_COUNTRY_REASON_ERROR_MESSAGE).WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.PII.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
            RuleFor(visitor => visitor.EmailAddress).Length(0, EMAIL_MAX_LENGTH).WithMessage(EMAIL_ERROR_MESSAGE).EmailAddress().WithMessage(INVALID_EMAIL_ERROR_MESSAGE).WithState(x => new ErrorPath { Category = ElementCategory.Person.ToString(), CategorySub = ElementCategorySub.PersonalInfo.ToString(), Section = ElementCategorySection.Contact.ToString(), Tab = ElementCategorySectionTab.PersonalInfo.ToString() });
        }
    }
}