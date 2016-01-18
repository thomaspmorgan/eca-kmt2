using System.Xml.Serialization;

namespace ECA.Business.Validation.Model
{
    public class DeleteDependent
    {
        public DeleteDependent()
        { }

        [XmlAttribute(AttributeName = "dependentSevisID")]
        public string dependentSevisID { get; set; }
    }
}