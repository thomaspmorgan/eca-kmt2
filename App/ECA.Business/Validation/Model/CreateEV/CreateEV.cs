using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(CreateEVValidator))]
    public class CreateEV
    {
        public CreateEV()
        {
            public ExchangeVisitor visitor { get; set; }
        }
    }
}
