using ECA.Business.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Controllers.Search
{
    public class ECASearchParametersBindingModel
    {
        public int Start { get; set; }
        public int Limit { get; set; }

        public string Filter { get; set; }
        public IEnumerable<string> Facets { get; set; }
        public IEnumerable<string> Fields { get; set; }
        public string SearchTerm { get; set; }

        public ECASearchParameters ToECASearchParameters()
        {
            return new ECASearchParameters(this.Start, this.Limit, this.Filter, this.Facets, this.Fields, this.SearchTerm);
        }
    }
}