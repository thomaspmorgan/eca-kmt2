using System;

namespace ECA.Business.Validation.Model.CreateEV
{
    /// <summary>
    /// Correct minor or technical infraction
    /// </summary>
    public class CorrectInfraction
    {
        public string InfractionType { get; set; }

        public DateTime NewEndDate { get; set; }

        public string Remarks { get; set; }

        public TippPhaseDates TippPhaseDates { get; set; }
    }
}