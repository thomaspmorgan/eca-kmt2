using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class EmailAddress : IHistorical
    {

        public const int EMAIL_ADDRESS_MAX_LENGTH = 100;

        [Key]
        public int EmailAddressId { get; set; }

        [MaxLength(EMAIL_ADDRESS_MAX_LENGTH)]
        [EmailAddress]
        public string Address { get; set; }

        public int EmailAddressTypeId { get; set; }

        public EmailAddressType EmailAddressType { get; set; }

        public History History { get; set; }

    }
}
