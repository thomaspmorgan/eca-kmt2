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
            this.Document = new ECADocumentViewModel(searchResult.Document);
            this.Score = searchResult.Score;
        }

        public ECADocumentViewModel Document { get; set; }

        public double Score { get; set; }
        
    }
}