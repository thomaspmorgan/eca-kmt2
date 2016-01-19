using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.CreateEV
{
    [Validator(typeof(BoardingSchoolValidator))]
    public class BoardingSchool
    {
        public BoardingSchool()
        { }

        [XmlElement(IsNullable = true)]
        public string Name { get; set; }

        [XmlElement(IsNullable = true)]
        public string Phone { get; set; }

        [XmlElement(IsNullable = true)]
        public string PhoneExt { get; set; }
    }
}