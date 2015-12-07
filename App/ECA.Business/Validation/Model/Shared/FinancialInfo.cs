using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(FinancialInfoValidator))]
    public class FinancialInfo
    {
        public string AcademicTerm { get; set; }

        public Expense Expense { get; set; }

        public Funding Funding { get; set; }
        
    }
}
