using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class EmailAddressType : IHistorical
    {
        private const int MAX_EMAIL_TYPE_LENGTH = 128;

        [Key]
        public int EmailAddressTypeId { get; set; }
        
        [MaxLength(MAX_EMAIL_TYPE_LENGTH)]
        public string EmailAddressTypeName { get; set; }

        public History History { get; set; }

    }
}
