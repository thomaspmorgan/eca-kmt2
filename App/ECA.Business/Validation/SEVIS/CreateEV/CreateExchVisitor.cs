using ECA.Business.Validation.Model.CreateEV;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// Exchange visitor record for create
    /// </summary>
    [Validator(typeof(CreateExchVisitorValidator))]
    public class CreateExchVisitor
    {
        public CreateExchVisitor()
        {
            //ExchangeVisitor = new ExchangeVisitor();
        }

        /// <summary>
        /// Exchange visitor information
        /// </summary>
        //public ExchangeVisitor ExchangeVisitor { get; set; }        
    }
}
