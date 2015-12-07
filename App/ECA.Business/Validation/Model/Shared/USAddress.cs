using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(USAddressValidator))]
    public class USAddress
    {
        public string address1 { get; set; }
        
        public string address2 { get; set; }
        
        public string city { get; set; }
        
        public string postalCode { get; set; }
        
        public string explanationCode { get; set; }

        public string explanation { get; set; }
    }
}
