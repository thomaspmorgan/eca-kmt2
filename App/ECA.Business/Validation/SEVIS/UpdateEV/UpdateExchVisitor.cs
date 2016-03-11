using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Exchange visitor record for update
    /// </summary>
    [Validator(typeof(UpdateExchVisitorValidator))]
    public class UpdateExchVisitor
    {
        public UpdateExchVisitor()
        {
            //ExchangeVisitor = new ExchangeVisitorUpdate();
        }

        /// <summary>
        /// Exchange visitor information
        /// </summary>
        //public ExchangeVisitorUpdate ExchangeVisitor { get; set; }
    }
}
