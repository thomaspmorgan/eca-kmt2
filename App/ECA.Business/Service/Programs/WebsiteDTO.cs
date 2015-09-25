using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Programs
{
    /// <summary>
    /// Data transfer object for website
    /// </summary>
    public class WebsiteDTO
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public WebsiteDTO(int? id, string value)
        {
            this.Id = id;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public int? Id { get; private set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; private set; }
    }
}
