using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class CancelProgramExtension
    {
        [Required]
        public bool printForm { get; set; }

        [MaxLength(10)]
        public DateTime NewPrgEndDate { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

    }
}
