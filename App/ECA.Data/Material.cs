using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class Material
    {
        [Key]
        public int MaterialId { get; set; }
        public string Name { get; set; }
    }
}
