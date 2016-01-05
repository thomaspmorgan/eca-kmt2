using ECA.Business.Validation.Model.CreateEV;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Exchange visitor record
    /// </summary>
    [Validator(typeof(CreateExchVisitorValidator))]
    public class CreateExchVisitor
    {
        /// <summary>
        /// Exchange visitor information
        /// </summary>
        public ExchangeVisitor visitor { get; set; }        
    }
}
