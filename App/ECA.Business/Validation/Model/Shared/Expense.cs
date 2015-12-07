using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ExpenseValidator))]
    public class Expense
    {
        public int Tuition { get; set; }
        
        public int LivingExpense { get; set; }
        
        public int DependentExp { get; set; }
        
        public ExpenseOther Other { get; set; }
    }
}
