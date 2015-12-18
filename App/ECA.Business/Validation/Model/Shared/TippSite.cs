using System;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(TippSiteValidator))]
    public class TippSite
    {
        public TippSite()
        {
            programOfficial = new ProgramOfficial();
            supervisors = new Supervisors();
        }

        public string siteId { get; set; }

        public ProgramOfficial programOfficial { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string postalCode { get; set; }

        public string explanationCode { get; set; }

        public string explanation { get; set; }

        public string siteName { get; set; }

        public bool primarySite { get; set; }
        
        public string Remarks { get; set; }

        public string employerId { get; set; }

        public string fullTimeEmployees { get; set; }

        public string annualRevenue { get; set; }

        public string websiteURL { get; set; }

        public bool workersCompInd { get; set; }

        public string WorkersCompCarrier { get; set; }

        public bool WorkersCompForEvInd { get; set; }

        public string EvHoursPerWeek { get; set; }

        public bool StipendInd { get; set; }

        public string stipendAmount { get; set; }

        public string stipendFrequency { get; set; }

        public string nonMonetaryComp { get; set; }

        public string supervisorLastName { get; set; }

        public string supervisorFirstName { get; set; }

        public string supervisorTitle { get; set; }

        public string supervisorPhone { get; set; }

        public string supervisorPhoneExt { get; set; }

        public string supervisorEmail { get; set; }

        public string supervisorFax { get; set; }

        public string officalUserName { get; set; }

        public DateTime officalSignatureDate { get; set; }

        public DateTime evSignatureDate { get; set; }

        public Supervisors supervisors { get; set; }
    }
}