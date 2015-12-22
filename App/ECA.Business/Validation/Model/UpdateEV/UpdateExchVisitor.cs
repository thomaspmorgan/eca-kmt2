using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Exchange visitor record
    /// </summary>
    [Validator(typeof(UpdateExchVisitorValidator))]
    public class UpdateExchVisitor
    {
        /// <summary>
        /// Exchange visitor information
        /// </summary>
        public ExchangeVisitorUpdate ExchangeVisitor { get; set; }
    }
}
