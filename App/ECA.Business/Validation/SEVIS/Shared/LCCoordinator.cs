using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(LCCoordinatorValidator))]
    public class LCCoordinator
    {
        public LCCoordinator()
        { }

        [XmlElement(IsNullable = true)]
        public string LastName { get; set; }

        [XmlElement(IsNullable = true)]
        public string FirsName { get; set; }
    }
}