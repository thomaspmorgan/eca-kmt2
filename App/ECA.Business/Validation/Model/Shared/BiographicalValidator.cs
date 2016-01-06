﻿using ECA.Business.Validation.Model.Shared;
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
            RuleFor(visitor => visitor.FullName).NotNull().WithMessage("Biographical Info: Full Name is required").SetValidator(new FullNameValidator()).When(visitor => visitor.FullName != null);
            RuleFor(visitor => visitor.BirthDate).NotNull().WithMessage("Biographical Info: Date of Birth is required");
            RuleFor(visitor => visitor.Gender).NotNull().WithMessage("Biographical Info: Gender is required").Length(GENDER_CODE_LENGTH).WithMessage("Biographical Info: Gender must be " + GENDER_CODE_LENGTH.ToString() + " character");
            RuleFor(visitor => visitor.BirthCity).NotNull().WithMessage("Biographical Info: City of Birth is required").Length(1, CITY_MAX_LENGTH).WithMessage("Biographical Info: City of Birth can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.BirthCountryCode).NotNull().WithMessage("Biographical Info: Country of Birth is required").Length(COUNTRY_CODE_LENGTH).WithMessage("Biographical Info: Country of Birth must be " + COUNTRY_CODE_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.CitizenshipCountryCode).NotNull().WithMessage("Biographical Info: Country of Citizenship is required").Length(COUNTRY_CODE_LENGTH).WithMessage("Biographical Info: Country of Citizenship must be " + COUNTRY_CODE_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.PermanentResidenceCountryCode).NotNull().WithMessage("Biographical Info: Permanent Residence Country is required").Length(COUNTRY_CODE_LENGTH).WithMessage("Biographical Info: Permanent Residence Country must be " + COUNTRY_CODE_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.BirthCountryReason).Length(0, BIRTH_COUNTRY_REASON_LENGTH).WithMessage("Biographical Info: Birth Country Reason must be " + BIRTH_COUNTRY_REASON_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.EmailAddress).Length(0, EMAIL_MAX_LENGTH).WithMessage("Biographical Info: Email can be up to " + EMAIL_MAX_LENGTH.ToString() + " characters").EmailAddress().WithMessage("Biographical Info: Email is invalid");
        }
    }
}