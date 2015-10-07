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
    /// The ECASuggestionParametersBindingModel is used to represent a client's request to get suggested search terms.
    /// </summary>
    public class ECASuggestionParametersBindingModel
    {
        /// <summary>
        /// The search term.
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// The highlight post tag.
        /// </summary>
        public string HighlightPostTag { get; set; }

        /// <summary>
        /// The highlight pre tag.
        /// </summary>
        public string HighlightPreTag { get; set; }

        /// <summary>
        /// Returns a business layer suggestion parameters business model.
        /// </summary>
        /// <param name="permissions">The user's permissions.</param>
        /// <returns>The business layer model.</returns>
        public ECASuggestionParameters ToECASuggestionParameters(IEnumerable<IPermission> permissions)
        {
            return new ECASuggestionParameters(this.SearchTerm, this.HighlightPreTag, this.HighlightPostTag);
        }
    }
}