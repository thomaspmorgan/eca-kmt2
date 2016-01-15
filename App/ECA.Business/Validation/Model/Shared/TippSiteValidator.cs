using ECA.Business.Validation.Model.CreateEV;
using FluentValidation;

namespace ECA.Business.Validation.Model
{
    public class TippSiteValidator : AbstractValidator<TippSite>
    {
        public const int SITEID_MAX_LENGTH = 50;
        public const int ADDRESS_MAX_LENGTH = 64;
        public const int CITY_MAX_LENGTH = 60;
        public const int STATE_LENGTH = 2;
        public const int POSTAL_CODE_LENGTH = 5;
        public const int EXPLANATION_CODE_LENGTH = 2;
        public const int EXPLANATION_MIN_LENGTH = 5;
        public const int EXPLANATION_MAX_LENGTH = 200;
        public const int SITE_MAX_LENGTH = 60;
        public const int REMARKS_MAX_LENGTH = 500;

        public TippSiteValidator()
        {
            RuleFor(visitor => visitor.SiteId).Length(0, SITEID_MAX_LENGTH).WithMessage("T/IPP Site: Site ID can be up to " + SITEID_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Address1).NotNull().WithMessage("T/IPP Site: Address Line 1 is required").Length(1, ADDRESS_MAX_LENGTH).WithMessage("T/IPP Site: Address Line 1 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Address2).Length(0, ADDRESS_MAX_LENGTH).WithMessage("T/IPP Site: Address Line 2 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.City).Length(0, CITY_MAX_LENGTH).WithMessage("T/IPP Site: City can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.State).Length(STATE_LENGTH).WithMessage("T/IPP Site: State code must be " + STATE_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.PostalCode).NotNull().WithMessage("T/IPP Site: Postal Code is required").Length(POSTAL_CODE_LENGTH).WithMessage("T/IPP Site: Postal Code must be " + POSTAL_CODE_LENGTH.ToString() + " digits").Matches(@" ^\d{5}$").WithMessage("T/IPP Site: Postal Code must be numeric");
            RuleFor(visitor => visitor.ExplanationCode).Length(EXPLANATION_CODE_LENGTH).WithMessage("T/IPP Site: Explanation Code must be " + EXPLANATION_CODE_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Explanation).Length(EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH).WithMessage("T/IPP Site: Explanation must be between " + EXPLANATION_MIN_LENGTH.ToString() + " and " + EXPLANATION_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.SiteName).NotNull().WithMessage("T/IPP Site: Site Name is required").Length(1, SITE_MAX_LENGTH).WithMessage("T/IPP Site: Site Name can be up to " + SITE_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.PrimarySite).NotNull().WithMessage("T/IPP Site: Primary Site Of Activity indicator is required");
            RuleFor(visitor => visitor.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("T/IPP Site: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.EmployerID).NotNull().WithMessage("T/IPP Site: Employer ID is required").Length(1, 9).WithMessage("T/IPP Site: Site Name can be up to 9 characters");
            RuleFor(visitor => visitor.FullTimeEmployees).NotNull().WithMessage("T/IPP Site: Number of full time employees is required").Length(1, 6).WithMessage("T/IPP Site: Number of full time employees must be between 1 and 6 characters");
            RuleFor(visitor => visitor.AnnualRevenue).NotNull().WithMessage("T/IPP Site: Annual Revenue amount is required").Length(1, 2).WithMessage("T/IPP Site: Annual Revenue amount can be up to 2 characters");
            RuleFor(visitor => visitor.WebsiteURL).NotNull().WithMessage("T/IPP Site: Website URL is required").Length(1, 250).WithMessage("T/IPP Site: Website URL can be up to 250 characters");
            RuleFor(visitor => visitor.WorkersCompInd).NotNull().WithMessage("T/IPP Site: Worker Compensation indicator is required");
            RuleFor(visitor => visitor.WorkersCompCarrier).Length(0, 100).WithMessage("T/IPP Site: Workers compensation carrier can be up to 100 characters");
            RuleFor(visitor => visitor.WorkersCompForEvInd).NotNull().WithMessage("T/IPP Site: Worker Compensation for exchange visitor indicator is required");
            RuleFor(visitor => visitor.EvHoursPerWeek).NotNull().WithMessage("T/IPP Site: Exchange visitor hours per week is required").Length(1, 2).WithMessage("T/IPP Site: Exchange visitor hours per week can be up to 2 characters");
            RuleFor(visitor => visitor.StipendInd).NotNull().WithMessage("T/IPP Site: Stipend indicator is required");
            RuleFor(visitor => visitor.StipendAmount).InclusiveBetween("0", "9999999999.99").WithMessage("T/IPP Site: Stipend amount can be up to $9999999999.99 USD");
            RuleFor(visitor => visitor.StipendFrequency).Length(0, 2).WithMessage("T/IPP Site: Stipend frequency can be up to 2 characters");
            RuleFor(visitor => visitor.NonMonetaryComp).Length(0, 100).WithMessage("T/IPP Site: Non monetary compensation can be up to 100 characters");
            RuleFor(visitor => visitor.SupervisorLastName).NotNull().WithMessage("T/IPP Site: Supervisor last name is required").Length(1, 40).WithMessage("T/IPP Site: Supervisor last name can be up to 40 characters");
            RuleFor(visitor => visitor.SupervisorFirstName).NotNull().WithMessage("T/IPP Site: Supervisor first name is required").Length(1, 40).WithMessage("T/IPP Site: Supervisor first name can be up to 40 characters");
            RuleFor(visitor => visitor.SupervisorTitle).NotNull().WithMessage("T/IPP Site: Supervisor title is required").Length(1, 100).WithMessage("T/IPP Site: Supervisor title can be up to 100 characters");
            RuleFor(visitor => visitor.SupervisorPhone).NotNull().WithMessage("T/IPP Site: Supervisor telephone number is required").Length(1, 10).WithMessage("T/IPP Site: Supervisor telephone number can be up to 10 characters");
            RuleFor(visitor => visitor.SupervisorPhoneExt).NotNull().WithMessage("T/IPP Site: Supervisor telephone number extenstion is required").Length(1, 4).WithMessage("T/IPP Site: Supervisor telephone number extenstion can be up to 4 characters");
            RuleFor(visitor => visitor.SupervisorEmail).NotNull().WithMessage("T/IPP Site: Supervisor Email address is required").Length(1, 255).WithMessage("T/IPP Site: Supervisor Email address can be up to 255 characters");
            RuleFor(visitor => visitor.SupervisorFax).Length(0, 10).WithMessage("T/IPP Site: Supervisor fax number can be up to 10 characters");
            RuleFor(visitor => visitor.OfficialUserName).Length(7, 10).WithMessage("T/IPP Site: Global username type must be between 7 and 10 characters");
        }
    }
}