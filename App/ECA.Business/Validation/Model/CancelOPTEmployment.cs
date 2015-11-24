﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class CancelOPTEmployment
    {
        [Required]
        public bool printForm { get; set; }

        [MaxLength(10)]
        public DateTime StartDate { get; set; }

        [MaxLength(10)]
        public DateTime EndDate { get; set; }

        [StringLength(2)]
        public string FullPartTimeIndicator { get; set; }

        [MaxLength(100)]
        public string EmployerName { get; set; }
        
    }
}
