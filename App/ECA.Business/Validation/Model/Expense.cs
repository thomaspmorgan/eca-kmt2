using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class Expense
    {
        [MaxLength(8)]
        public int Tuition { get; set; }

        [MaxLength(8)]
        public int LivingExpense { get; set; }
        
        [MaxLength(8)]
        public int DependentExp { get; set; }
        
        public ExpenseOther Other { get; set; }
    }
}
