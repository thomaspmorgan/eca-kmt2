using System;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.CreateEV
{
    public class ProgramOfficial
    {
        public ProgramOfficial()
        { }

        public string UserName { get; set; }

        [XmlElement(IsNullable = true)]
        public DateTime SignatureDate { get; set; }
    }
}