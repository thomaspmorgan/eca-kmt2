﻿using System;

namespace ECA.Business.Validation.Model.Shared
{
    /// <summary>
    /// correct minor or technical infractions
    /// </summary>
    public class CorrectInfraction
    {
        public CorrectInfraction()
        {
            TippPhaseDates = new TippPhaseDatesUpdate();
        }

        public string InfractionType { get; set; }

        public DateTime NewEndDate { get; set; }

        public string Remarks { get; set; }

        /// <summary>
        /// T/IPP phase dates
        /// </summary>
        public TippPhaseDatesUpdate TippPhaseDates { get; set; }
    }
}