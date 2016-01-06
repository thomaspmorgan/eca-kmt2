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
            RuleFor(student => student.printForm).NotNull().WithMessage("Site of Activity: Print form option is required");
            RuleFor(student => student.Address1).Length(1, ADDRESS_MAX_LENGTH).WithMessage("Site of Activity: Address Line 1 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Address2).Length(0, ADDRESS_MAX_LENGTH).WithMessage("Site of Activity: Address Line 2 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.City).Length(0, CITY_MAX_LENGTH).WithMessage("Site of Activity: City can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.State).Length(STATE_CODE_LENGTH).WithMessage("Site of Activity: State Code must be " + STATE_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.PostalCode).Length(POSTAL_CODE_LENGTH).WithMessage("Site of Activity: Postal Code must be " + POSTAL_CODE_LENGTH.ToString() + " digits");
            RuleFor(student => student.ExplanationCode).Length(EXPLANATION_CODE_LENGTH).WithMessage("Site of Activity: Explanation Code must be " + EXPLANATION_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.Explanation).Length(EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH).WithMessage("Site of Activity: Explanation must be between " + EXPLANATION_MIN_LENGTH.ToString() + " and " + EXPLANATION_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.SiteName).NotNull().WithMessage("Site of Activity: Site Name is required").Length(1, SITE_MAX_LENGTH).WithMessage("T/IPP Site: Site Name can be up to " + SITE_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.PrimarySite).NotNull().WithMessage("Site of Activity: Primary Site Of Activity indicator is required");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("Site of Activity: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
        }
    }
}