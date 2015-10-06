using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// The ECASearchParameters is a wrapper object for the Azure Search Parameters.
    /// </summary>
    public class ECASearchParameters
    {
        /// <summary>
        /// Creates a new ECASearchParameters with the search parameters themselves.
        /// </summary>
        /// <param name="start">The number of records to skip.</param>
        /// <param name="limit">The number of records to return.</param>
        /// <param name="filter">The Azure Search OData filter parameter.</param>
        /// <param name="facets">The search facets.</param>
        /// <param name="selectFields">The names of the fields to return.</param>
        /// <param name="searchTerm">The search term or keyword.</param>
        /// <param name="highlightPostTag">The hit highlight post tag.</param>
        /// <param name="highlightPreTag">The hit highlight pre tag.</param>
        public ECASearchParameters(int start, int limit, string filter, IEnumerable<string> facets, IEnumerable<string> selectFields, string searchTerm, string highlightPreTag, string highlightPostTag)
        {
            this.Start = start;
            this.Limit = limit;
            this.Facets = facets;
            this.SelectFields = selectFields;
            this.Filter = filter;
            this.SearchTerm = searchTerm;
            this.HighlightPostTag = highlightPostTag;
            this.HighlightPreTag = highlightPreTag;
        }

        /// <summary>
        /// Gets the start.
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// Gets the limit.
        /// </summary>
        public int Limit { get; private set; }

        /// <summary>
        /// Gets the facets.
        /// </summary>
        public IEnumerable<string> Facets { get; private set; }

        /// <summary>
        /// Gets the fields to select.
        /// </summary>
        public IEnumerable<string> SelectFields { get; private set; }

        /// <summary>
        /// Gets the search term.
        /// </summary>
        public string SearchTerm { get; private set; }

        /// <summary>
        /// Gets OData azure search filter.
        /// </summary>
        public string Filter { get; private set; }

        /// <summary>
        /// Gets the highlight pre tag.
        /// </summary>
        public string HighlightPreTag { get; private set; }

        /// <summary>
        /// Gets the highlight post tag.
        /// </summary>
        public string HighlightPostTag { get; private set; }
    }
}
