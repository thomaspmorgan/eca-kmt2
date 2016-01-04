using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// Update financial info
    /// </summary>
    [Validator(typeof(FinancialInfoUpdateValidator))]
    public class FinancialInfoUpdate : FinancialInfo
    {
        public FinancialInfoUpdate()
        { }

        /// <summary>
        /// Print request indicator
        /// </summary>
        public bool printForm { get; set; }
    }
}
