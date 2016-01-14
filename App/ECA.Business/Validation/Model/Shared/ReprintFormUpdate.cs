using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(ReprintFormUpdateValidator))]
    public class ReprintFormUpdate : ReprintForm
    {
        public ReprintFormUpdate()
        { }

        /// <summary>
        /// Dependent Sevis ID
        /// </summary>
        [XmlAttribute(AttributeName = "dependentSevisID")]
        public string dependentSevisID { get; set; }
    }
}