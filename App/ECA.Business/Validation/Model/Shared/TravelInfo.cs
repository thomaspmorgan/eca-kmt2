using System;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(TravelInfoValidator))]
    public class TravelInfo
    {
        public TravelInfo()
        { }

        public string PassportNumber { get; set; }
        
        public string PassportIssuingCntry { get; set; }

        public DateTime PassportExpDate { get; set; }
        
        public string VisaNumber { get; set; }
        
        public string VisaIssuingCntry { get; set; }

        public DateTime VisaIssueDate { get; set; }

        public DateTime VisaExpDate { get; set; }
        
        public string PortOfEntry { get; set; }

        public DateTime DateOfEntry { get; set; }        
    }
}
