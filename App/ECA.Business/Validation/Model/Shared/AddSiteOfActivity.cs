using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model.CreateEV
{
    /// <summary>
    /// Site of activity address
    /// </summary>
    [Validator(typeof(AddSiteOfActivityValidator))]
    public class AddSiteOfActivity
    {
        public AddSiteOfActivity()
        {
            SiteOfActivitySOA = new SiteOfActivitySOA();
            SiteOfActivityExempt = new SiteOfActivityExempt();
        }

        /// <summary>
        /// xsi:type = SOA
        /// </summary>
        public SiteOfActivitySOA SiteOfActivitySOA { get; set; }

        /// <summary>
        /// xsi:type = EXEMPT
        /// </summary>
        public SiteOfActivityExempt SiteOfActivityExempt { get; set; }        
    }
}