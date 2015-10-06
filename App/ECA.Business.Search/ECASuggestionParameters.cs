using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// The ECASuggestionParameters is used by a business layer client to retrieve suggested search terms.
    /// </summary>
    public class ECASuggestionParameters
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="highlightPreTag">The highlight pre tag.</param>
        /// <param name="highlightPostTag">The post highlight tag.</param>
        public ECASuggestionParameters(string searchTerm, string highlightPreTag, string highlightPostTag)
        {
            this.SearchTerm = searchTerm;
            this.HighlightPostTag = highlightPostTag;
            this.HighlightPreTag = highlightPreTag;
        }

        /// <summary>
        /// Gets the search term.
        /// </summary>
        public string SearchTerm { get; private set; }

        /// <summary>
        /// Gets the highlight post tag.
        /// </summary>
        public string HighlightPostTag { get; private set; }

        /// <summary>
        /// Gets the highlight pre tag.
        /// </summary>
        public string HighlightPreTag { get; private set; }
    }
}
