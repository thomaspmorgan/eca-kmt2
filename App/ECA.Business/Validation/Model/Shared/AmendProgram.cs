using System;
using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.Shared
{
    [Validator(typeof(AmendProgramValidator))]
    public class AmendProgram
    {
        public AmendProgram()
        {
            TippPhaseDates = new TippPhaseDates();
        }

        [XmlAttribute(AttributeName = "printForm")]
        public bool printForm { get; set; }

        public DateTime PrgStartDate { get; set; }

        public DateTime PrgEndDate { get; set; }

        [XmlElement(IsNullable = true)]
        public string Remarks { get; set; }

        /// <summary>
        /// T/IPP phase dates for exch visitor
        /// </summary>
        public TippPhaseDates TippPhaseDates { get; set; }
    }
}