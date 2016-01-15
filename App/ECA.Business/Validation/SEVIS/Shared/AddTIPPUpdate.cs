using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// T/IPP site with phases
    /// </summary>
    [Validator(typeof(AddTIPPUpdateValidator))]
    public class AddTIPPUpdate
    {
        public AddTIPPUpdate()
        {
            ParticipantInfo = new ParticipantInfoUpdate();
            TippSite = new TippSiteUpdate();
        }

        [XmlAttribute(AttributeName = "print7002")]
        public bool print7002 { get; set; }

        public ParticipantInfoUpdate ParticipantInfo { get; set; }

        public TippSiteUpdate TippSite { get; set; }
    }
}