using ECA.Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// A DocumentSaveActionConfiguration is used to configure a SaveAction for a TDocument.  For example,
    /// rules can be created for when an entity should be ignored by the DocumentsSaveAction such as an Organization
    /// that is not an Office, when Offices are being documented.
    /// 
    /// A DocumentSaveActionConfiguration can also be used for entities that themselves are not documented but instead related entities are.
    /// 
    /// For example, an Address is not itself indexed, but its related Organization is.  A DocumentSaveAction typed for Addresses
    /// will need to return the Organization Id and and Organization Type Id for the DocumentSaveAction to create the correct queue message.
    /// 
    /// 
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    public class DocumentSaveActionConfiguration<TDocument>
        where TDocument : class
    {
        /// <summary>
        /// Creates a new instance with the functions for selecting the proper Id and document type id.  The rules will be initialized
        /// but have no items.
        /// </summary>
        /// <param name="idSelector">The expression to select TDocument's id.</param>
        /// <param name="documentTypeId">The document type id.</param>
        public DocumentSaveActionConfiguration(Expression<Func<TDocument, object>> idSelector, Guid documentTypeId)
        {
            Contract.Requires(idSelector != null, "The id selector must not be null.");
            Contract.Requires(documentTypeId != Guid.Empty, "The document type id must not be the empty guid.");
            this.IdDelegate = idSelector.Compile();
            this.DocumentTypeIdDelegate = (instance) => documentTypeId;
            this.CreatedExclusionRules = new List<Func<TDocument, bool>>();
            this.ModifiedExclusionRules = new List<Func<TDocument, bool>>();
            this.DeletedExclusionRules = new List<Func<TDocument, bool>>();
        }

        /// <summary>
        /// Creates a new instance with the given expressions/functions.  The rules will be initialized
        /// but have no items.
        /// </summary>
        /// <param name="idSelector">The expression to select TDocument's id.</param>
        /// <param name="documentTypeIdDelegate">The function to return TDocument's document type id.</param>
        public DocumentSaveActionConfiguration(Expression<Func<TDocument, object>> idSelector, Func<TDocument, Guid> documentTypeIdDelegate)
        {
            Contract.Requires(idSelector != null, "The id selector must not be null.");
            Contract.Requires(documentTypeIdDelegate != null, "The documentTypeIdDelegate must not be null.");
            this.IdDelegate = idSelector.Compile();
            this.DocumentTypeIdDelegate = documentTypeIdDelegate;
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

        /// <summary>
        /// Gets or sets the id delegate used to retrieve the id from the TDocument.
        /// </summary>
        public Func<TDocument, object> IdDelegate { get; private set; }

        /// <summary>
        /// Gets the document type id delegate.
        /// </summary>
        public Func<TDocument, Guid> DocumentTypeIdDelegate { get; private set; }

        /// <summary>
        /// Returns the Id of the given instance.
        /// </summary>
        /// <param name="instance">The id of the instance.</param>
        /// <returns>The id.</returns>
        public object GetId(TDocument instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Assert(this.IdDelegate != null, "The id delegate should not be null here.");
            return IdDelegate(instance);
        }

        /// <summary>
        /// Returns the document type id.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The document type id.</returns>
        public Guid GetDocumentTypeId(TDocument instance)
        {
            Contract.Requires(instance != null, "The instance must not be null.");
            Contract.Assert(this.DocumentTypeIdDelegate != null, "The document type id delegate should not be null here.");
            return DocumentTypeIdDelegate(instance);
        }
    }
}
