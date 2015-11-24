using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class StudentRegistration
    {
        [Required]
        public bool printForm { get; set; }

        public bool LastSession { get; set; }

        [MaxLength(10)]
        public DateTime CurrentSessionEndDate { get; set; }

        [MaxLength(10)]
        public DateTime NextSessionStartDate { get; set; }

        public bool Commuter { get; set; }

        public USAddress usAddress { get; set; }

        public ForeignAddress foreignAddress { get; set; }

        public TravelInfo travelInfo { get; set; }
        
        [MaxLength(500)]
        public string Remarks { get; set; }
    }
}
