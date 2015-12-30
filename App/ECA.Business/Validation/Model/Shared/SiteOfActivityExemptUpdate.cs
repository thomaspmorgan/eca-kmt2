using ECA.Business.Validation.Model.CreateEV;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(SiteOfActivityExemptUpdateValidator))]
    public class SiteOfActivityExemptUpdate : SiteOfActivityExempt
    {
        public SiteOfActivityExemptUpdate()
        { }

        /// <summary>
        /// Print request indicator
        /// </summary>
        public bool printForm { get; set; }
    }
}