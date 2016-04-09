using ECA.Business.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ECA.WebApi.Models.Search
{
    /// <summary>
    /// 
    /// </summary>
    public class DocumentSuggestResponseViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public DocumentSuggestResponseViewModel(DocumentSuggestResponse<ECADocument> response)
        {
            Contract.Requires(response != null, "The response must not be null.");
            this.Documents = new List<SuggestResultViewModel>();
            foreach(var result in response.Results)
            {
                this.Documents.Add(new SuggestResultViewModel(result));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<SuggestResultViewModel> Documents { get; set; }
    }
}