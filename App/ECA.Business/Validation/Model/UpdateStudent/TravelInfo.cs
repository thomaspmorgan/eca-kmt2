using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class TravelInfo
    {
        [MaxLength(25)]
        public string PassportNumber { get; set; }

        [StringLength(2)]
        public string PassportIssuingCntry { get; set; }

        public DateTime PassportExpDate { get; set; }

        [MaxLength(25)]
        public string VisaNumber { get; set; }

        [MaxLength(3)]
        public string VisaIssuingCntry { get; set; }

        public DateTime VisaIssueDate { get; set; }

        public DateTime VisaExpDate { get; set; }

        [MaxLength(3)]
        public string PortOfEntry { get; set; }

        public DateTime DateOfEntry { get; set; }
        
    }
}
