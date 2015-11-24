using System;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class CancelDependent
    {
        [MaxLength(11)]
        public string dependentSevisID { get; set; }

        [StringLength(2)]
        public string Reason { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

    }
}
