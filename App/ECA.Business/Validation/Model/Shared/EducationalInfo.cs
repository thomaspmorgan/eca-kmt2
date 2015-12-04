using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(EducationalInfoValidator))]
    public class EducationalInfo
    {
        public EduLevel eduLevel { get; set; }
        
        public string PrimaryMajor { get; set; }
        
        public string SecondMajor { get; set; }
        
        public string Minor { get; set; }
        
        public string LengthOfStudy { get; set; }

        public DateTime PrgStartDate { get; set; }

        public DateTime PrgEndDate { get; set; }

        public EngProficiency engProficiency { get; set; }
        
        public string Remarks { get; set; }
    }
}
