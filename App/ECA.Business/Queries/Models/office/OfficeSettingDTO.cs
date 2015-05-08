using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Office
{
    public class OfficeSettingDTO
    {
        public int Id { get; set; }

        public int OfficeId { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
