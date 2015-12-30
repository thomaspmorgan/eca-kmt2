using FluentValidation;

namespace ECA.Business.Validation.Model.Shared
{
    public class EditTippSiteValidator : AbstractValidator<EditTippSite>
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

        public EditTippSiteValidator()
        {
            RuleFor(student => student.SiteId).Length(0, SITEID_MAX_LENGTH).WithMessage("T/IPP Site: Site ID can be up to " + SITEID_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Address1).NotNull().WithMessage("T/IPP Site: Address Line 1 is required").Length(1, ADDRESS_MAX_LENGTH).WithMessage("T/IPP Site: Address Line 1 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.Address2).Length(0, ADDRESS_MAX_LENGTH).WithMessage("T/IPP Site: Address Line 2 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.City).Length(0, CITY_MAX_LENGTH).WithMessage("T/IPP Site: City can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.State).Length(STATE_LENGTH).WithMessage("T/IPP Site: State code must be " + STATE_LENGTH.ToString() + " characters");
            RuleFor(student => student.PostalCode).NotNull().WithMessage("T/IPP Site: Postal Code is required").Length(POSTAL_CODE_LENGTH).WithMessage("T/IPP Site: Postal Code must be " + POSTAL_CODE_LENGTH.ToString() + " digits").Matches(@" ^\d{5}$").WithMessage("T/IPP Site: Postal Code must be numeric");
            RuleFor(student => student.ExplanationCode).Length(EXPLANATION_CODE_LENGTH).WithMessage("T/IPP Site: Explanation Code must be " + EXPLANATION_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.Explanation).Length(EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH).WithMessage("T/IPP Site: Explanation must be between " + EXPLANATION_MIN_LENGTH.ToString() + " and " + EXPLANATION_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.SiteName).NotNull().WithMessage("T/IPP Site: Site Name is required").Length(1, SITE_MAX_LENGTH).WithMessage("T/IPP Site: Site Name can be up to " + SITE_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.PrimarySite).NotNull().WithMessage("T/IPP Site: Primary Site Of Activity indicator is required");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("T/IPP Site: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.EmployerID).NotNull().WithMessage("T/IPP Site: Employer ID is required").Length(1, 9).WithMessage("T/IPP Site: Site Name can be up to 9 characters");
            RuleFor(student => student.FullTimeEmployees).NotNull().WithMessage("T/IPP Site: Number of full time employees is required").Length(1, 6).WithMessage("T/IPP Site: Number of full time employees must be between 1 and 6 characters");
            RuleFor(student => student.AnnualRevenue).NotNull().WithMessage("T/IPP Site: Annual Revenue amount is required").Length(1, 2).WithMessage("T/IPP Site: Annual Revenue amount can be up to 2 characters");
            RuleFor(student => student.WebsiteURL).NotNull().WithMessage("T/IPP Site: Website URL is required").Length(1, 250).WithMessage("T/IPP Site: Website URL can be up to 250 characters");
            RuleFor(student => student.WorkersCompInd).NotNull().WithMessage("T/IPP Site: Worker Compensation indicator is required");
            RuleFor(student => student.WorkersCompCarrier).Length(0, 100).WithMessage("T/IPP Site: Workers compensation carrier can be up to 100 characters");
            RuleFor(student => student.WorkersCompForEvInd).NotNull().WithMessage("T/IPP Site: Worker Compensation for exchange visitor indicator is required");
            RuleFor(student => student.EvHoursPerWeek).NotNull().WithMessage("T/IPP Site: Exchange visitor hours per week is required").Length(1, 2).WithMessage("T/IPP Site: Exchange visitor hours per week can be up to 2 characters");
            RuleFor(student => student.StipendInd).NotNull().WithMessage("T/IPP Site: Stipend indicator is required");
            RuleFor(student => student.StipendAmount).InclusiveBetween("0", "9999999999.99").WithMessage("T/IPP Site: Stipend amount can be up to $9999999999.99 USD");
            RuleFor(student => student.StipendFrequency).Length(0, 2).WithMessage("T/IPP Site: Stipend frequency can be up to 2 characters");
            RuleFor(student => student.NonMonetaryComp).Length(0, 100).WithMessage("T/IPP Site: Non monetary compensation can be up to 100 characters");
            RuleFor(student => student.SupervisorLastName).NotNull().WithMessage("T/IPP Site: Supervisor last name is required").Length(1, 40).WithMessage("T/IPP Site: Supervisor last name can be up to 40 characters");
            RuleFor(student => student.SupervisorFirstName).NotNull().WithMessage("T/IPP Site: Supervisor first name is required").Length(1, 40).WithMessage("T/IPP Site: Supervisor first name can be up to 40 characters");
            RuleFor(student => student.SupervisorTitle).NotNull().WithMessage("T/IPP Site: Supervisor title is required").Length(1, 100).WithMessage("T/IPP Site: Supervisor title can be up to 100 characters");
            RuleFor(student => student.SupervisorPhone).NotNull().WithMessage("T/IPP Site: Supervisor telephone number is required").Length(1, 10).WithMessage("T/IPP Site: Supervisor telephone number can be up to 10 characters");
            RuleFor(student => student.SupervisorPhoneExt).NotNull().WithMessage("T/IPP Site: Supervisor telephone number extenstion is required").Length(1, 4).WithMessage("T/IPP Site: Supervisor telephone number extenstion can be up to 4 characters");
            RuleFor(student => student.SupervisorEmail).NotNull().WithMessage("T/IPP Site: Supervisor Email address is required").Length(1, 255).WithMessage("T/IPP Site: Supervisor Email address can be up to 255 characters");
            RuleFor(student => student.SupervisorFax).Length(0, 10).WithMessage("T/IPP Site: Supervisor fax number can be up to 10 characters");
            RuleFor(student => student.OfficialUserName).Length(7, 10).WithMessage("T/IPP Site: Global username type must be between 7 and 10 characters");
        }
    }
}