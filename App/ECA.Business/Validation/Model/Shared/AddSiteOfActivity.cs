using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

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
        /// Site of activity address
        /// </summary>
        [XmlElement("SiteOfActivity xsi:type=\"SOA\"")]
        public SiteOfActivitySOA SiteOfActivitySOA { get; set; }

        /// <summary>
        /// Site of activity information for exempt
        /// </summary>
        [XmlElement("SiteOfActivity xsi:type=\"EXEMPT\"")]
        public SiteOfActivityExempt SiteOfActivityExempt { get; set; }
    }
}