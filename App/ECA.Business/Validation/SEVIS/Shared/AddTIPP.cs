using FluentValidation.Attributes;
using System.Xml.Serialization;

namespace ECA.Business.Validation.Model.CreateEV
{
    /// <summary>
    /// T/IPP information
    /// </summary>
    [Validator(typeof(AddTippValidator))]
    public class AddTIPP
    {
        public AddTIPP()
        {
            TippExemptProgram = new TippExemptProgram();
            ParticipantInfo = new ParticipantInfo();
            TippSite = new TippSite();
        }

        /// <summary>
        /// print ds-7002 indicator
        /// </summary>
        [XmlAttribute(AttributeName = "print7002")]
        public bool print7002 { get; set; }

        /// <summary>
        /// T/IPP exempt information
        /// </summary>
        public TippExemptProgram TippExemptProgram { get; set; }

        /// <summary>
        /// T/IPP program participant information
        /// </summary>
        public ParticipantInfo ParticipantInfo { get; set; }

        /// <summary>
        /// T/IPP site with phases
        /// </summary>
        public TippSite TippSite { get; set; }
    }

    /// <summary>
    /// An EcaAddTIPP class is used when the ECA ignores TIPP information.
    /// </summary>
    public class EcaAddTIPP : AddTIPP
    {
        /// <summary>
        /// Creates a new default instance.
        /// </summary>
        public EcaAddTIPP()
        {
            this.print7002 = false;
            this.TippExemptProgram = null;
            this.ParticipantInfo = null;
            this.TippSite = null;
        }
    }
}