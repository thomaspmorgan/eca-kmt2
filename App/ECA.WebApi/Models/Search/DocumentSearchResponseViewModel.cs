using ECA.Business.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Search
{
    public class DocumentSearchResponseViewModel
    {
        public DocumentSearchResponseViewModel(DocumentSearchResponse<ECADocument> response)
        {
            this.Count = response.Count;
            this.Coverage = response.Coverage;
            this.Results = new List<SearchResultViewModel>();
            foreach(var result in response.Results)
            {
                this.Results.Add(new SearchResultViewModel(result));
            }
        }

        public long? Count { get; set; }

        public double? Coverage { get; set; }

        //public FacetResults Facets { get; set; }

        public List<SearchResultViewModel> Results { get; set; }

    }
}