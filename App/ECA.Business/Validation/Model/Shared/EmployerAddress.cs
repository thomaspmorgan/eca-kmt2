using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(EmployerAddressValidator))]
    public class EmployerAddress
    {
        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }
        
        public string State { get; set; }
        
        public string PostalCode { get; set; }
        
        public string PostalRoundingCode { get; set; }
    }
}
