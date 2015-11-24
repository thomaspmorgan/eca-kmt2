using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class EditAuthDrop
    {
        /// <summary>
        /// Print form flag
        /// </summary>
        [Required]
        public bool printForm { get; set; }

        [StringLength(2)]
        public string Reason { get; set; }

        [MaxLength(10)]
        public DateTime StartDate { get; set; }

        [MaxLength(10)]
        public DateTime EndDate { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

        [StringLength(2)]
        public string NewReason { get; set; }

        [MaxLength(10)]
        public DateTime NewStartDate { get; set; }

        [MaxLength(10)]
        public DateTime NewEndDate { get; set; }

    }
}
