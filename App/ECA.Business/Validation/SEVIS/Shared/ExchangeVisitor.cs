using System;
using FluentValidation.Attributes;
using ECA.Business.Validation.Model.Shared;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.CreateEV
{
    /// <summary>
    /// Exchange visitor information
    /// </summary>
    [Validator(typeof(ExchangeVisitorValidator))]
    public class ExchangeVisitor
    {
        public ExchangeVisitor()
        {
            Biographical = new Biographical();
            SubjectField = new SubjectField();
            USAddress = new USAddress();
            MailAddress = new USAddress();
            FinancialInfo = new FinancialInfo();
            CreateDependent = new CreateDependent();
            AddSiteOfActivity = new AddSiteOfActivity();
            AddTIPP = new AddTIPP();
            ResidentialAddress = new ResidentialAddress();
        }

        /// <summary>
        /// Request identifier
        /// </summary>
        [XmlAttribute(AttributeName = "requestID")]
        public string requestID { get; set; }

        /// <summary>
        /// SEVIS user id
        /// </summary>
        [XmlAttribute(AttributeName = "userID")]
        public string userID { get; set; }

        /// <summary>
        /// Print request indicator
        /// </summary>
        [XmlAttribute(AttributeName = "printForm")]
        public bool printForm { get; set; }

        /// <summary>
        /// User defined field A
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string UserDefinedA { get; set; }

        /// <summary>
        /// User defined field B
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string UserDefinedB { get; set; }

        /// <summary>
        /// Biographical information
        /// </summary>
        public Biographical Biographical { get; set; }

        /// <summary>
        /// Position code (numeric)
        /// </summary>
        public string PositionCode { get; set; }
        
        /// <summary>
        /// Program start date
        /// </summary>
        public DateTime PrgStartDate { get; set; }

        /// <summary>
        /// Program end date
        /// </summary>
        public DateTime? PrgEndDate { get; set; }

        /// <summary>
        /// Program category
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// Occupational category code
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string OccupationCategoryCode { get; set; }

        /// <summary>
        /// Subject or field of study
        /// </summary>
        public SubjectField SubjectField { get; set; }

        /// <summary>
        /// U.S. physical address
        /// </summary>
        public USAddress USAddress { get; set; }

        /// <summary>
        /// U.S. mailing address
        /// </summary>
        public USAddress MailAddress { get; set; }

        /// <summary>
        /// Financial support information
        /// </summary>
        public FinancialInfo FinancialInfo { get; set; }
        
        /// <summary>
        /// Dependent information
        /// </summary>
        public CreateDependent CreateDependent { get; set; }

        /// <summary>
        /// Site of activity address
        /// </summary>
        public AddSiteOfActivity AddSiteOfActivity { get; set; }

        /// <summary>
        /// Add T/IPP information
        /// </summary>
        public AddTIPP AddTIPP { get; set; }

        /// <summary>
        /// Residential address information
        /// </summary>
        public ResidentialAddress ResidentialAddress { get; set; }
        
    }
}