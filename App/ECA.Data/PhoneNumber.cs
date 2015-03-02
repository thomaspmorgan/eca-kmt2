using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class PhoneNumber
    {
        [Key]
        public int PhoneNumberId { get; set; }
        [Phone]
        public string Number { get; set; }
        public PhoneNumberType PhoneNumberType { get; set; }
        public int PhoneNumberTypeId { get; set; }
    } 
}
