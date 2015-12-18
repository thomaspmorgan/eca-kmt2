using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(SiteOfActivityValidator))]
    public class SiteOfActivity
    {
        public string xsitype { get; set; }

        public bool printForm { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string postalCode { get; set; }

        public string explanationCode { get; set; }

        public string explanation { get; set; }

        public string siteName { get; set; }

        public bool primarySite { get; set; }

        public string remarks { get; set; }

        public DeleteSiteOfActivity Delete { get; set; }

        public EditSiteOfActivity Edit { get; set; }
    }
}