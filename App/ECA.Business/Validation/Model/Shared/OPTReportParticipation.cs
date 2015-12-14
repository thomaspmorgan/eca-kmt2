using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(OPTReportParticipationValidator))]
    public class OPTReportParticipation
    {
        public bool printForm { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string FullPartTimeIndicator { get; set; }
    }
}
