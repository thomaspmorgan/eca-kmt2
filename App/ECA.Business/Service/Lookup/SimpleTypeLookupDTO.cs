using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// Extension of the SimpleLookupDTO to add Type property
    /// </summary>
    public class SimpleTypeLookupDTO : SimpleLookupDTO
    {
        /// <summary>
        /// Gets or set generic type property
        /// </summary>
        public string Type { get; set; }
    }
}
