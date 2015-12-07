using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ChangeEducationLevelValidator))]
    public class ChangeEducationLevel
    {
        public ChangeEducationLevel()
        {
            educationalInfo = new EducationalInfo();
            financialInfo = new FinancialInfo();
        }

        public bool printForm { get; set; }

        public EducationalInfo educationalInfo { get; set; }

        public FinancialInfo financialInfo { get; set; }
        
        public string Remarks { get; set; }
    }
}
