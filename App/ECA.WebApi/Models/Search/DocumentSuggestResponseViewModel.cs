using ECA.Business.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Search
{
    public class DocumentSuggestResponseViewModel
    {
        public DocumentSuggestResponseViewModel(DocumentSuggestResponse<ECADocument> response)
        {
            Contract.Requires(response != null, "The response must not be null.");
            this.Documents = new List<SuggestResultViewModel>();
            foreach(var result in response.Results)
            {
                this.Documents.Add(new SuggestResultViewModel(result));
            }
        }

        public IList<SuggestResultViewModel> Documents { get; set; }
    }
}