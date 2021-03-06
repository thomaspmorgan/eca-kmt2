﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// The IndexDocumentBatchMessage is a message sent to the azure queue to notify added, updated, and deleted documents by id.
    /// </summary>
    public class IndexDocumentBatchMessage
    {
        /// <summary>
        /// Creates a new default instance and initializes the document key properties.
        /// 
        /// If the code is built in debug mode the IsDebugMessage property will be set to true.  Debug Messages
        /// will be ignored via the webjob.
        /// </summary>
        public IndexDocumentBatchMessage()
        {
            this.CreatedDocuments = new List<string>();
            this.DeletedDocuments = new List<string>();
            this.ModifiedDocuments = new List<string>();
            this.IsDebugMessage = false;
#if DEBUG
            this.IsDebugMessage = true;
#endif
        }

        /// <summary>
        /// Gets or sets the document keys of entities that have been created.
        /// </summary>
        public IEnumerable<string> CreatedDocuments { get; set; }

        /// <summary>
        /// Gets or sets the document keys of entities that have been updated.
        /// </summary>
        public IEnumerable<string> ModifiedDocuments { get; set; }

        /// <summary>
        /// Gets or sets the document keys of entities that have been deleted.
        /// </summary>
        public IEnumerable<string> DeletedDocuments { get; set; }

        /// <summary>
        /// Gets or sets whether this message was generated in debug configured code.
        /// </summary>
        public bool IsDebugMessage { get; set; }

        /// <summary>
        /// Returns true if this message has documents to index.
        /// </summary>
        /// <returns>True, if this message has documents to index, otherwise, false.</returns>
        public bool HasDocumentsToHandle()
        {
            return this.CreatedDocuments.Count() > 0
                || this.DeletedDocuments.Count() > 0
                || this.ModifiedDocuments.Count() > 0;
        }
    }
}
