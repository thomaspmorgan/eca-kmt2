using CAM.Business.Service;
using ECA.Business.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Search
{
    /// <summary>
    /// An ECASearchParametersBindingModel is used to bind to a client's request for a full text search against Azure Search.
    /// </summary>
    public class ECASearchParametersBindingModel
    {
        /// <summary>
        /// The number of records to skip.
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// The number of records to return.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// The OData Azure Search filter value.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// The search facets, currently only supports 'documentTypeName'.
        /// </summary>
        public IEnumerable<string> Facets { get; set; }

        /// <summary>
        /// The fields to select and return in the search, such as id, name, description, documentType.
        /// </summary>
        public IEnumerable<string> SelectFields { get; set; }

        /// <summary>
        /// The query.
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// The hit highlight pre tag.
        /// </summary>
        public string HighlightPreTag { get; set; }

        /// <summary>
        /// The hit highlight post tag.
        /// </summary>
        public string HighlightPostTag { get; set; }

        /// <summary>
        /// Returns an ECASearchParameters instance for use in the business layer.
        /// </summary>
        /// <param name="permissions">The current user's permissions.</param>
        /// <returns>The ECASearchParameters.</returns>
        public ECASearchParameters ToECASearchParameters(IEnumerable<IPermission> permissions)
        {
            Contract.Requires(permissions != null, "The permissions must not be null.");
            return new ECASearchParameters(
                this.Start, 
                this.Limit, 
                this.Filter, 
                this.Facets, 
                this.SelectFields, 
                this.SearchTerm, 
                this.HighlightPreTag, 
                this.HighlightPostTag
                );
        }
    }
}