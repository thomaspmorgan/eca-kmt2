using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(SiteOfActivitySOAValidator))]
    public class SiteOfActivitySOA
    {
        public SiteOfActivitySOA()
        { }

        public bool printForm { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string ExplanationCode { get; set; }

        public string Explanation { get; set; }

        public string SiteName { get; set; }

        public bool PrimarySite { get; set; }

        public string Remarks { get; set; }
    }
}