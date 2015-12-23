using ECA.Business.Validation.Model.CreateEV;
using System.Xml.Serialization;
using FluentValidation.Attributes;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// Site of activity events
    /// </summary>
    [Validator(typeof(SiteOfActivityUpdateValidator))]
    public class SiteOfActivityUpdate
    {
        /// <summary>
        /// Add site of activity
        /// </summary>
        [XmlArrayItem("Add xsi:type=\"AddSOA\"")]
        public SiteOfActivitySOA AddSOA { get; set; }

        /// <summary>
        /// Site of activity indicating site is exempt
        /// </summary>
        [XmlArrayItem("Add xsi:type=\"AddEXEMPT\"")]
        public SiteOfActivityExemptUpdate AddEXEMPT { get; set; }

        /// <summary>
        /// Site of activity indicating site is seeking employment
        /// </summary>
        [XmlArrayItem("Add xsi:type=\"AddSEEKINGEMP\"")]
        public SiteOfActivitySeekingGemp AddSEEKINGEMP { get; set; }

        /// <summary>
        /// Site of activity indicating site is on travel
        /// </summary>
        [XmlArrayItem("Add xsi:type=\"AddONTRAVEL\"")]
        public SiteOfActivityAddOnTravel AddONTRAVEL { get; set; }

        /// <summary>
        /// Delete site of activity
        /// </summary>
        public DeleteSiteOfActivity Delete { get; set; }

        /// <summary>
        /// Edit site of activity
        /// </summary>
        public EditSiteOfActivity Edit { get; set; }        
    }
}