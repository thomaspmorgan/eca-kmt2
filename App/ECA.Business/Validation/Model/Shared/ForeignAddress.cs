using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(ForeignAddressValidator))]
    public class ForeignAddress
    {
        public string address1 { get; set; }
        
        public string address2 { get; set; }
        
        public string city { get; set; }
        
        public string province { get; set; }
        
        public string countryCode { get; set; }
        
        public string postalCode { get; set; }
    }
}
