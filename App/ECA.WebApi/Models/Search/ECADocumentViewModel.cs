using ECA.Business.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Search
{
    public class ECADocumentViewModel
    {
        public ECADocumentViewModel()
        {

        }

        public ECADocumentViewModel(ECADocument document)
        {
            this.Key = document.GetKey();
            this.Document = document;
        }

        public DocumentKey Key { get; set; }
        public ECADocument Document { get; set; }
    }
}