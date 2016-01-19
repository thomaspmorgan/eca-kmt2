using System;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model
{
    public class TerminateDependent
    {
        public TerminateDependent()
        { }

        /// <summary>
        /// Dependent Sevis ID
        /// </summary>
        [XmlAttribute(AttributeName = "dependentSevisID")]
        public string dependentSevisID { get; set; }

        public string Reason { get; set; }
        
        public DateTime EffectiveDate { get; set; }

        [XmlElement(IsNullable = true)]
        public string OtherRemarks { get; set; }

        [XmlElement(IsNullable = true)]
        public string Remarks { get; set; }        
    }
}
