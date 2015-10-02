using ECA.Business.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Search
{
    /// <summary>
    /// A SearchResultViewModel
    /// </summary>
    public class SearchResultViewModel
    {
        /// <summary>
        /// Creates a new SearchResultViewModel given the Azure SearchResult ECADocument.
        /// </summary>
        /// <param name="searchResult">The search result.</param>
        public SearchResultViewModel(SearchResult<ECADocument> searchResult)
        {
            Contract.Requires(searchResult != null, "The search result must not be null.");
            Contract.Requires(searchResult.Document != null, "The document must not be null.");
            this.Document = new ECADocumentViewModel(searchResult.Document);
            this.Score = searchResult.Score;
            this.HitHighlights = searchResult.Highlights;
        }

        /// <summary>
        /// The document.
        /// </summary>
        public ECADocumentViewModel Document { get; set; }

        /// <summary>
        /// The document score.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// The search result highlights.
        /// </summary>
        public Dictionary<string, IList<string>> HitHighlights { get; set; }        
    }
}