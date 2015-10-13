using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// A DocumentSaveActionConfiguration is used to configure a SaveAction for a TDocument.
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    public class DocumentSaveActionConfiguration<TDocument>
        where TDocument : class, IIdentifiable
    {
        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        public DocumentSaveActionConfiguration()
        {
            this.CreatedExclusionRules = new List<Func<TDocument, bool>>();
            this.ModifiedExclusionRules = new List<Func<TDocument, bool>>();
            this.DeletedExclusionRules = new List<Func<TDocument, bool>>();
        }

        /// <summary>
        /// Gets or sets the exclusion rules for created TDocuments.  Add an exclusion rule to keep a created document from being processed by a DocumentSaveAction.
        /// </summary>
        public IEnumerable<Func<TDocument, bool>> CreatedExclusionRules { get; set; }

        /// <summary>
        /// Gets or sets the exclusion rules for modified TDocuments.  Add an exclusion rule to keep a modified document from being processed by a DocumentSaveAction.
        /// </summary>
        public IEnumerable<Func<TDocument, bool>> ModifiedExclusionRules { get; set; }

        /// <summary>
        /// Gets or sets the exclusion rules for deleted TDocuments.  Add an exclusion rule to keep a deleted document from being processed by a DocumentSaveAction.
        /// </summary>
        public IEnumerable<Func<TDocument, bool>> DeletedExclusionRules { get; set; }
    }
}
