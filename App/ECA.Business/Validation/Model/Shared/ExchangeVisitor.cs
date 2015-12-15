using System;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(ExchangeVisitorValidator))]
    public class ExchangeVisitor
    {
        public ExchangeVisitor()
        {
            biographical = new PersonalInfo();
            subjectField = new SubjectField();
            usAddress = new USAddress();
            mailAddress = new USAddress();
            financialInfo = new FinancialInfo();
            createDependent = new CreateDependent();
            addSiteOfActivity = new AddSiteOfActivity();
            addTIPP = new AddTIPP();
            residentialAddress = new ResidentialAddress();
        }

        public bool isNew { get; set; }

        public string requestID { get; set; }

        public string userID { get; set; }

        public bool printForm { get; set; }

        public string UserDefinedA { get; set; }

        public string UserDefinedB { get; set; }

        public PersonalInfo biographical { get; set; }

        public string positionCode { get; set; }
        
        public DateTime PrgStartDate { get; set; }

        public DateTime? PrgEndDate { get; set; }

        public string categoryCode { get; set; }

        public string occupationCategoryCode { get; set; }

        public SubjectField subjectField { get; set; }
        
        public USAddress usAddress { get; set; }
        
        public USAddress mailAddress { get; set; }

        public FinancialInfo financialInfo { get; set; }
        
        public CreateDependent createDependent { get; set; }
        
        public AddSiteOfActivity addSiteOfActivity { get; set; }

        public AddTIPP addTIPP { get; set; }

        public ResidentialAddress residentialAddress { get; set; }        
    }
}