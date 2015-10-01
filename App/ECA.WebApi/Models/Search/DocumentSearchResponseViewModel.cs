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
    /// A DocumentSearchResponseViewModel is used by a client to view search results from an Azure Search 
    /// full text search.
    /// </summary>
    public class DocumentSearchResponseViewModel
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="response">The azure search document search response.</param>
        public DocumentSearchResponseViewModel(DocumentSearchResponse<ECADocument> response)
        {
            Contract.Requires(response != null, "The response must not be null.");
            this.Count = response.Count;
            this.Coverage = response.Coverage;
            this.Results = new List<SearchResultViewModel>();
            foreach (var result in response.Results)
            {
                this.Results.Add(new SearchResultViewModel(result));
            }
        }

        /// <summary>
        /// The count of documents.
        /// </summary>
        public long? Count { get; set; }

        /// <summary>
        /// The coverage.
        /// </summary>
        public double? Coverage { get; set; }

        /// <summary>
        /// The search results.
        /// </summary>
        public List<SearchResultViewModel> Results { get; set; }
    }
}