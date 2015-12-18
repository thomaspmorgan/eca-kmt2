using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(AddSiteOfActivityValidator))]
    public class AddSiteOfActivity
    {
        public AddSiteOfActivity()
        {
            siteOfActivity = new SiteOfActivity();
        }

        /// <summary>
        /// xsi:type = SOA or EXEMPT
        /// </summary>
        public SiteOfActivity siteOfActivity { get; set; }
    }
}