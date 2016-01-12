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
            RuleFor(visitor => visitor.Address1).Length(1, ADDRESS_MAX_LENGTH).WithMessage("T/IPP Site: Address Line 1 is required and can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Address2).Length(0, ADDRESS_MAX_LENGTH).WithMessage("T/IPP Site: Address Line 2 can be up to " + ADDRESS_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.City).Length(0, CITY_MAX_LENGTH).WithMessage("T/IPP Site: City can be up to " + CITY_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.State).Length(STATE_LENGTH).WithMessage("T/IPP Site: State code must be " + STATE_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.PostalCode).Length(POSTAL_CODE_LENGTH).WithMessage("T/IPP Site: Postal Code is required and must be " + POSTAL_CODE_LENGTH.ToString() + " digits").Matches(@" ^\d{5}$").WithMessage("T/IPP Site: Postal Code must be numeric");
            RuleFor(visitor => visitor.ExplanationCode).Length(EXPLANATION_CODE_LENGTH).WithMessage("T/IPP Site: Explanation Code is required and must be " + EXPLANATION_CODE_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.Explanation).Length(EXPLANATION_MIN_LENGTH, EXPLANATION_MAX_LENGTH).WithMessage("T/IPP Site: Explanation must be between " + EXPLANATION_MIN_LENGTH.ToString() + " and " + EXPLANATION_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.SiteName).Length(1, SITE_MAX_LENGTH).WithMessage("T/IPP Site: Site Name can be up to " + SITE_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.PrimarySite).NotNull().WithMessage("T/IPP Site: Primary Site Of Activity indicator is required");
            RuleFor(visitor => visitor.Remarks).Length(0, REMARKS_MAX_LENGTH).WithMessage("T/IPP Site: Remarks can be up to " + REMARKS_MAX_LENGTH.ToString() + " characters");
            RuleFor(visitor => visitor.EmployerID).Length(1, 9).WithMessage("T/IPP Site: Site Name is required and can be up to 9 characters");
            RuleFor(visitor => visitor.FullTimeEmployees).Length(1, 6).WithMessage("T/IPP Site: Number of Full Time Employees is required and must be between 1 and 6 characters");
            RuleFor(visitor => visitor.AnnualRevenue).Length(1, 2).WithMessage("T/IPP Site: Annual Revenue Amount is required and can be up to 2 characters");
            RuleFor(visitor => visitor.WebsiteURL).Length(1, 250).WithMessage("T/IPP Site: Website URL can be up to 250 characters");
            RuleFor(visitor => visitor.WorkersCompInd).NotNull().WithMessage("T/IPP Site: Worker Compensation indicator is required");
            RuleFor(visitor => visitor.WorkersCompCarrier).Length(0, 100).WithMessage("T/IPP Site: Workers Compensation Carrier can be up to 100 characters");
            RuleFor(visitor => visitor.WorkersCompForEvInd).NotNull().WithMessage("T/IPP Site: Worker Compensation for Exch. Visitor indicator is required");
            RuleFor(visitor => visitor.EvHoursPerWeek).Length(1, 2).WithMessage("T/IPP Site: Exch. Visitor Hours Per Week can be up to 2 characters");
            RuleFor(visitor => visitor.StipendInd).NotNull().WithMessage("T/IPP Site: Stipend indicator is required");
            RuleFor(visitor => visitor.StipendAmount).InclusiveBetween("0", "9999999999.99").WithMessage("T/IPP Site: Stipend Amount can be up to $9999999999.99 USD");
            RuleFor(visitor => visitor.StipendFrequency).Length(0, 2).WithMessage("T/IPP Site: Stipend Frequency can be up to 2 characters");
            RuleFor(visitor => visitor.NonMonetaryComp).Length(0, 100).WithMessage("T/IPP Site: Non Monetary Compensation can be up to 100 characters");
            RuleFor(visitor => visitor.SupervisorLastName).Length(1, 40).WithMessage("T/IPP Site: Supervisor Last Name is required and can be up to 40 characters");
            RuleFor(visitor => visitor.SupervisorFirstName).Length(1, 40).WithMessage("T/IPP Site: Supervisor First Name is required and can be up to 40 characters");
            RuleFor(visitor => visitor.SupervisorTitle).Length(1, 100).WithMessage("T/IPP Site: Supervisor Title is required and can be up to 100 characters");
            RuleFor(visitor => visitor.SupervisorPhone).Length(1, 10).WithMessage("T/IPP Site: Supervisor Phone Number is required and can be up to 10 characters");
            RuleFor(visitor => visitor.SupervisorPhoneExt).Length(0, 4).WithMessage("T/IPP Site: Supervisor Phone Number Extenstion can be up to 4 characters");
            RuleFor(visitor => visitor.SupervisorEmail).Length(1, 255).WithMessage("T/IPP Site: Supervisor Email Address is required and can be up to 255 characters");
            RuleFor(visitor => visitor.SupervisorFax).Length(0, 10).WithMessage("T/IPP Site: Supervisor Fax Number can be up to 10 characters");
            RuleFor(visitor => visitor.OfficialUserName).Length(7, 10).WithMessage("T/IPP Site: Global Username Type must be between 7 and 10 characters");
        }
    }
}