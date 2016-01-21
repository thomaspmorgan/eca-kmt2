using FluentValidation;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class SiteOfActivitySOAValidator : AbstractValidator<SiteOfActivitySOA>
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

        public SiteOfActivitySOAValidator()
        {
            RuleFor(visitor => visitor.printForm).NotNull().WithMessage("Site of Activity: Print form option is required");
            RuleFor(visitor => visitor.Address1).Length(1, ADDRESS_MAX_LENGTH).WithMessage("Site of Activity: Address Line 1 is required and can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Address2).Length(0, ADDRESS_MAX_LENGTH).WithMessage("Site of Activity: Address Line 2 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.City).Length(0, CITY_MAX_LENGTH).WithMessage("Site of Activity: City can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.State).Length(STATE_CODE_LENGTH).WithMessage("Site of Activity: State Code is required and must be " + STATE_CODE_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.PostalCode).Length(POSTAL_CODE_LENGTH).WithMessage("Site of Activity: Postal Code is required and must be " + POSTAL_CODE_LENGTH.ToString() + " digits");
            RuleFor(visitor => visitor.ExplanationCode).Length(EXPLANATION_CODE_LENGTH).WithMessage("Site of Activity: Explanation Code is required and must be " + EXPLANATION_CODE_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Explanation).Length(EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH).WithMessage("Site of Activity: Explanation is required and must be between " + EXPLANATION_MIN_LENGTH.ToString() + " and " + EXPLANATION_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.SiteName).Length(1, SITE_MAX_LENGTH).WithMessage("T/IPP Site: Site Name is required and can be up to " + SITE_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.PrimarySite).NotNull().WithMessage("Site of Activity: Primary Site Of Activity indicator is required");
            RuleFor(visitor => visitor.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Site of Activity: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}