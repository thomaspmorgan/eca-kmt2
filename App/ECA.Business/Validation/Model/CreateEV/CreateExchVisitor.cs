using ECA.Business.Validation.Model.CreateEV;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(CreateExchVisitorValidator))]
    public class CreateExchVisitor
    {
        public ExchangeVisitor visitor { get; set; }        
    }
}
