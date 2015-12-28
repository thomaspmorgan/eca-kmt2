using System;

namespace ECA.Business.Validation.Model
{
    public class AmendProgram
    {
        public bool printForm { get; set; }

        public DateTime PrgStartDate { get; set; }

        public DateTime PrgEndDate { get; set; }

        public string Remarks { get; set; }

        public TippPhaseDates TippPhaseDates { get; set; }
    }
}