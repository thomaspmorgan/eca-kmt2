using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Search
{
    /// <summary>
    /// The DeleteIndexBindingModel represents an administrator's request to delete an azure search index.
    /// </summary>
    public class DeleteIndexBindingModel
    {
        /// <summary>
        /// The application the search index is associated to.  KMT is 1.
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// The name of the index to delete.
        /// </summary>
        public string IndexName { get; set; }
    }
}