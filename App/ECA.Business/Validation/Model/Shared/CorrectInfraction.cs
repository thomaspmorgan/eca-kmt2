using System;

namespace ECA.Business.Validation.Model.Shared
{
    public class CorrectInfraction
    {
        public CorrectInfraction()
        {
            TippPhaseDates = new TippPhaseDatesUpdate();
        }

        public string InfractionType { get; set; }

        public DateTime NewEndDate { get; set; }

        public string Remarks { get; set; }

        public TippPhaseDatesUpdate TippPhaseDates { get; set; }
    }
}