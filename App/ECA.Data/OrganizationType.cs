using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{

    public class OrganizationType
    {
        [Key]
        public int OrganizationTypeId { get; set; }
        [Required]
        public string OrganizationTypeName { get; set; }

        public History History { get; set; }
    }

}
