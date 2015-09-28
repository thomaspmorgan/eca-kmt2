using ECA.Business.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Search
{
    public class SearchResultViewModel
    {
        public SearchResultViewModel(SearchResult<ECADocument> searchResult)
        {
            this.Key = searchResult.Document.GetKey();
            this.Document = searchResult.Document;
            this.Score = searchResult.Score;
        }

        public DocumentKey Key { get; set; }

        public ECADocument Document { get; set; }

        public double Score { get; set; }
        
    }
}