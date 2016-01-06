using FluentValidation.Attributes;
using System;

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
}