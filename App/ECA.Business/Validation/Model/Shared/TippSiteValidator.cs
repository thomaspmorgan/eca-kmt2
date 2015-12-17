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
            RuleFor(student => student.siteId).Length(0, SITEID_MAX_LENGTH).WithMessage("T/IPP Site: Site ID can be up to " + SITEID_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.address1).NotNull().WithMessage("T/IPP Site: Address Line 1 is required").Length(1, ADDRESS_MAX_LENGTH).WithMessage("T/IPP Site: Address Line 1 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.address2).Length(0, ADDRESS_MAX_LENGTH).WithMessage("T/IPP Site: Address Line 2 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.city).Length(0, CITY_MAX_LENGTH).WithMessage("T/IPP Site: City can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.state).Length(STATE_LENGTH).WithMessage("T/IPP Site: State code must be " + STATE_LENGTH.ToString() + " characters");
            RuleFor(student => student.postalCode).NotNull().WithMessage("T/IPP Site: Postal Code is required").Length(POSTAL_CODE_LENGTH).WithMessage("T/IPP Site: Postal Code must be " + POSTAL_CODE_LENGTH.ToString() + " digits").Matches(@" ^\d{5}$").WithMessage("T/IPP Site: Postal Code must be numeric");
            RuleFor(student => student.explanationCode).Length(EXPLANATION_CODE_LENGTH).WithMessage("T/IPP Site: Explanation Code must be " + EXPLANATION_CODE_LENGTH.ToString() + " characters");
            RuleFor(student => student.explanation).Length(EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH).WithMessage("T/IPP Site: Explanation must be between " + EXPLANATION_MIN_LENGTH.ToString() + " and " + EXPLANATION_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.siteName).NotNull().WithMessage("T/IPP Site: Site Name is required").Length(1, SITE_MAX_LENGTH).WithMessage("T/IPP Site: Site Name can be up to " + SITE_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.primarySite).NotNull().WithMessage("T/IPP Site: Primary Site Of Activity indicator is required");
            RuleFor(student => student.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("T/IPP Site: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
            RuleFor(student => student.employerId).NotNull().WithMessage("T/IPP Site: Employer ID is required").Length(1, 9).WithMessage("T/IPP Site: Site Name can be up to 9 characters");
            RuleFor(student => student.fullTimeEmployees).NotNull().WithMessage("T/IPP Site: Number of full time employees is required").Length(1, 6).WithMessage("T/IPP Site: Number of full time employees must be between 1 and 6 characters");
            RuleFor(student => student.annualRevenue).NotNull().WithMessage("T/IPP Site: Annual Revenue amount is required").Length(1, 2).WithMessage("T/IPP Site: Annual Revenue amount can be up to 2 characters");
            RuleFor(student => student.websiteURL).NotNull().WithMessage("T/IPP Site: Website URL is required").Length(1, 250).WithMessage("T/IPP Site: Website URL can be up to 250 characters");
            RuleFor(student => student.workersCompInd).NotNull().WithMessage("T/IPP Site: Worker Compensation indicator is required");
            RuleFor(student => student.WorkersCompCarrier).Length(0, 100).WithMessage("T/IPP Site: Workers compensation carrier can be up to 100 characters");
            RuleFor(student => student.WorkersCompForEvInd).NotNull().WithMessage("T/IPP Site: Worker Compensation for exchange visitor indicator is required");
            RuleFor(student => student.EvHoursPerWeek).NotNull().WithMessage("T/IPP Site: Exchange visitor hours per week is required").Length(1, 2).WithMessage("T/IPP Site: Exchange visitor hours per week can be up to 2 characters");
            RuleFor(student => student.StipendInd).NotNull().WithMessage("T/IPP Site: Stipend indicator is required");
            RuleFor(student => student.stipendAmount).InclusiveBetween("0", "9999999999.99").WithMessage("T/IPP Site: Stipend amount can be up to $9999999999.99 USD");
            RuleFor(student => student.stipendFrequency).Length(0, 2).WithMessage("T/IPP Site: Stipend frequency can be up to 2 characters");
            RuleFor(student => student.nonMonetaryComp).Length(0, 100).WithMessage("T/IPP Site: Non monetary compensation can be up to 100 characters");
            RuleFor(student => student.supervisorLastName).NotNull().WithMessage("T/IPP Site: Supervisor last name is required").Length(1, 40).WithMessage("T/IPP Site: Supervisor last name can be up to 40 characters");
            RuleFor(student => student.supervisorFirstName).NotNull().WithMessage("T/IPP Site: Supervisor first name is required").Length(1, 40).WithMessage("T/IPP Site: Supervisor first name can be up to 40 characters");
            RuleFor(student => student.supervisorTitle).NotNull().WithMessage("T/IPP Site: Supervisor title is required").Length(1, 100).WithMessage("T/IPP Site: Supervisor title can be up to 100 characters");
            RuleFor(student => student.supervisorPhone).NotNull().WithMessage("T/IPP Site: Supervisor telephone number is required").Length(1, 10).WithMessage("T/IPP Site: Supervisor telephone number can be up to 10 characters");
            RuleFor(student => student.supervisorPhoneExt).NotNull().WithMessage("T/IPP Site: Supervisor telephone number extenstion is required").Length(1, 4).WithMessage("T/IPP Site: Supervisor telephone number extenstion can be up to 4 characters");
            RuleFor(student => student.supervisorEmail).NotNull().WithMessage("T/IPP Site: Supervisor Email address is required").Length(1, 255).WithMessage("T/IPP Site: Supervisor Email address can be up to 255 characters");
            RuleFor(student => student.supervisorFax).Length(0, 10).WithMessage("T/IPP Site: Supervisor fax number can be up to 10 characters");
            RuleFor(student => student.officalUserName).Length(7, 10).WithMessage("T/IPP Site: Global username type must be between 7 and 10 characters");
            RuleFor(student => student.tippPhase).SetValidator(new TippPhaseValidator()).When(student => student.tippPhase != null);
        }
    }
}