using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ExpenseOtherValidator))]
    public class ExpenseOther
    {
        public int Amount { get; set; }
        
        public string Remarks { get; set; }
    }
}
