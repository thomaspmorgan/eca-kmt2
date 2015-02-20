using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class PhoneNumberType
    {
        [Key]
        public int PhoneNumberTypeId { get; set; }
        [Required]
        [MaxLength(20)]
        public string PhoneNumberTypeName { get; set; }
        public History History { get; set; }
    }
}

