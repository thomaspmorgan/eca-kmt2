using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    /// <summary>
    /// U.S. physical address
    /// </summary>
    [Validator(typeof(USAddressValidator))]
    public class USAddress
    {
        public USAddress()
        { }

        public string Address1 { get; set; }
        
        public string Address2 { get; set; }
        
        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }
        
        public string ExplanationCode { get; set; }

        public string Explanation { get; set; }
    }
}
