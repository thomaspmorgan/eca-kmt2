using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(SContactValidator))]
    public class SContact
    {
        public SContact()
        { }

        [XmlElement(IsNullable = true)]
        public string LastName { get; set; }

        [XmlElement(IsNullable = true)]
        public string FirsName { get; set; }
    }
}