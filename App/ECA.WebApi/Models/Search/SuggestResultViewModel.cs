using ECA.Business.Search;
using Microsoft.Azure.Search.Models;
using System.Diagnostics.Contracts;

namespace ECA.WebApi.Models.Search
{
    /// <summary>
    /// 
    /// </summary>
    public class SuggestResultViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public SuggestResultViewModel(SuggestResult<ECADocument> suggestedResult)
        {
            Contract.Requires(suggestedResult != null, "The suggested result must not be null.");
            this.Document = new ECADocumentViewModel(suggestedResult.Document);
            this.Text = suggestedResult.Text;
        }

        /// <summary>
        /// The suggested document.
        /// </summary>
        public ECADocumentViewModel Document { get; set; }

        /// <summary>
        /// The suggested text.
        /// </summary>
        public string Text { get; set; }
    }
}