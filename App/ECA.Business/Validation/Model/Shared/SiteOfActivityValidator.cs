using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class SiteOfActivityValidator : AbstractValidator<SiteOfActivity>
    {
        public const int ADDRESS_MAX_LENGTH = 64;
        public const int CITY_MAX_LENGTH = 60;
        public const int STATE_CODE_LENGTH = 2;
        public const int POSTAL_CODE_LENGTH = 5;
        public const int EXPLANATION_CODE_LENGTH = 2;
        public const int EXPLANATION_MIN_LENGTH = 5;
        public const int EXPLANATION_MAX_LENGTH = 200;
        public const int SITE_MAX_LENGTH = 60;
        public const int REMARKS_MAX_LENGTH = 500;

        public SiteOfActivityValidator()
        {
            RuleFor(student => student.printForm).NotNull().WithMessage("Site of Activity: Print form option is required");
            RuleFor(student => student.address1).Length(1, ADDRESS_MAX_LENGTH).WithMessage("Site of Activity: Address Line 1 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.address2).Length(0, ADDRESS_MAX_LENGTH).WithMessage("Site of Activity: Address Line 2 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.city).Length(0, CITY_MAX_LENGTH).WithMessage("Site of Activity: City can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.state).Length(STATE_CODE_LENGTH).WithMessage("Site of Activity: State Code must be " + STATE_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.postalCode).Length(POSTAL_CODE_LENGTH).WithMessage("Site of Activity: Postal Code must be " + POSTAL_CODE_LENGTH.ToString() + " digits").Matches(@" ^\d{5}$").WithMessage("Site of Activity: Postal Code must be numeric");
            RuleFor(student => student.explanationCode).Length(EXPLANATION_CODE_LENGTH).WithMessage("Site of Activity: Explanation Code must be " + EXPLANATION_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.explanation).Length(EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH).WithMessage("Site of Activity: Explanation must be between " + EXPLANATION_MIN_LENGTH.ToString() + " and " + EXPLANATION_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.siteName).NotNull().WithMessage("Site of Activity: Site Name is required").Length(1, SITE_MAX_LENGTH).WithMessage("T/IPP Site: Site Name can be up to " + SITE_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.primarySite).NotNull().WithMessage("Site of Activity: Primary Site Of Activity indicator is required");
            RuleFor(student => student.remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Site of Activity: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}