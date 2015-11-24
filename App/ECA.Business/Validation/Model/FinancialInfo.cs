using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class FinancialInfo
    {
        [MaxLength(2)]
        public string AcademicTerm { get; set; }

        public Expense Expense { get; set; }

        public Funding Funding { get; set; }
        
    }
}
