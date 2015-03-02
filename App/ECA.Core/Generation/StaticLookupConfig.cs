using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.Generation
{
    /// <summary>
    /// A StaticLookupConfig is a config object used for generating static lookup objects via text template.
    /// </summary>
    public class StaticLookupConfig
    {
        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the class name.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the Id column name.
        /// </summary>
        public string IdColumnName { get; set; }

        /// <summary>
        /// Gets or sets the name of value column.
        /// </summary>
        public string ValueColumnName { get; set; }
    }
}
