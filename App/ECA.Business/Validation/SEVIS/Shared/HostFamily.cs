using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(HostFamilyValidator))]
    public class HostFamily
    {
        public HostFamily()
        {
            PContact = new PContact();
            SContact = new SContact();
        }

        public PContact PContact { get; set; }

        public SContact SContact { get; set; }

        [XmlElement(IsNullable = true)]
        public string Phone { get; set; }

        [XmlElement(IsNullable = true)]
        public string PhoneExt { get; set; }
    }
}