using System;

namespace ECA.Business.Validation.Model.Shared
{
    public class EditTippSite
    {
        public string SiteId { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string ExplanationCode { get; set; }

        public string Explanation { get; set; }

        public string SiteName { get; set; }

        public bool PrimarySite { get; set; }

        public string Remarks { get; set; }

        public string EmployerID { get; set; }

        public string FullTimeEmployees { get; set; }

        public string AnnualRevenue { get; set; }

        public string WebsiteURL { get; set; }

        public bool WorkersCompInd { get; set; }

        public string WorkersCompCarrier { get; set; }

        public bool WorkersCompForEvInd { get; set; }

        public string EvHoursPerWeek { get; set; }

        public bool StipendInd { get; set; }

        public string StipendAmount { get; set; }

        public string StipendFrequency { get; set; }

        public string NonMonetaryComp { get; set; }

        public string SupervisorLastName { get; set; }

        public string SupervisorFirstName { get; set; }

        public string SupervisorTitle { get; set; }

        public string SupervisorPhone { get; set; }

        public string SupervisorPhoneExt { get; set; }

        public string SupervisorEmail { get; set; }

        public string SupervisorFax { get; set; }

        public string OfficialUserName { get; set; }

        public DateTime OfficialSignatureDate { get; set; }

        public DateTime EvSignatureDate { get; set; }

        public SupervisorsUpdate Supervisors { get; set; }
    }
}