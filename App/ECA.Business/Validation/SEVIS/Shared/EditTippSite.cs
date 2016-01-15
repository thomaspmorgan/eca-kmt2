using System;
using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(EditTippSiteValidator))]
    public class EditTippSite
    {
        public EditTippSite()
        {
            Supervisors = new SupervisorsUpdate();
        }

        public string SiteId { get; set; }

        public string Address1 { get; set; }

        [XmlElement(IsNullable = true)]
        public string Address2 { get; set; }

        [XmlElement(IsNullable = true)]
        public string City { get; set; }

        [XmlElement(IsNullable = true)]
        public string State { get; set; }

        public string PostalCode { get; set; }

        [XmlElement(IsNullable = true)]
        public string ExplanationCode { get; set; }

        [XmlElement(IsNullable = true)]
        public string Explanation { get; set; }

        public string SiteName { get; set; }

        public bool PrimarySite { get; set; }

        [XmlElement(IsNullable = true)]
        public string Remarks { get; set; }

        public string EmployerID { get; set; }

        public string FullTimeEmployees { get; set; }

        public string AnnualRevenue { get; set; }

        public string WebsiteURL { get; set; }

        public bool WorkersCompInd { get; set; }

        [XmlElement(IsNullable = true)]
        public string WorkersCompCarrier { get; set; }

        public bool WorkersCompForEvInd { get; set; }

        public string EvHoursPerWeek { get; set; }

        public bool StipendInd { get; set; }

        [XmlElement(IsNullable = true)]
        public string StipendAmount { get; set; }

        [XmlElement(IsNullable = true)]
        public string StipendFrequency { get; set; }

        [XmlElement(IsNullable = true)]
        public string NonMonetaryComp { get; set; }

        public string SupervisorLastName { get; set; }

        public string SupervisorFirstName { get; set; }

        public string SupervisorTitle { get; set; }

        public string SupervisorPhone { get; set; }

        [XmlElement(IsNullable = true)]
        public string SupervisorPhoneExt { get; set; }

        public string SupervisorEmail { get; set; }

        [XmlElement(IsNullable = true)]
        public string SupervisorFax { get; set; }

        [XmlElement(IsNullable = true)]
        public string OfficialUserName { get; set; }

        [XmlElement(IsNullable = true)]
        public DateTime OfficialSignatureDate { get; set; }

        [XmlElement(IsNullable = true)]
        public DateTime EvSignatureDate { get; set; }

        public SupervisorsUpdate Supervisors { get; set; }
    }
}