using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(SiteOfActivityExemptValidator))]
    public class SiteOfActivityExempt
    {
        public SiteOfActivityExempt()
        { }

        public string Remarks { get; set; }
    }
}