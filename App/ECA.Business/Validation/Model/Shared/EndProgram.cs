using System;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class EndProgram
    {
        public EndProgram()
        { }

        public string Reason { get; set; }

        public DateTime EffectiveDate { get; set; }

        [XmlElement(IsNullable = true)]
        public string Remarks { get; set; }
    }
}