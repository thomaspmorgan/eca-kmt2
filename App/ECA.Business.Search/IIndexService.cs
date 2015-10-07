using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Search.Models;
using System.Diagnostics.Contracts;

namespace ECA.Business.Search
{
    /// <summary>
    /// An IndexService is capable of indexing data so that it may be searched quickly.
    /// </summary>
    [ContractClass(typeof(IndexServiceContract))]
    public interface IIndexService
    {
        /// <summary>
        /// Returns the document configuration for the given type.
        /// </summary>
        /// <typeparam name="T">The type T to retrieve the document configuration for.</typeparam>
        /// <returns>The document configuration.</returns>
        IDocumentConfiguration GetDocumentConfiguration<T>();

        /// <summary>
        /// Creates an index for the type T.
        /// </summary>
        /// <typeparam name="T">The document type T.</typeparam>
        void CreateIndex<T>() where T : class;

        /// <summary>
        /// Creates an index for the type T.
        /// </summary>
        /// <typeparam name="T">The document type T.</typeparam>
        Task CreateIndexAsync<T>() where T : class;

        /// <summary>
        /// Deletes an index with the given name.
        /// </summary>
        /// <param name="indexName">The index name.</param>
        /// <returns>The task.</returns>
        Task DeleteIndexAsync(string indexName);

        /// <summary>
        /// Deletes an index with the given name.
        /// </summary>
        /// <param name="indexName">The index name.</param>
        void DeleteIndex(string indexName);

        /// <summary>
        /// Returns the document with the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The document.</returns>
        Task<ECADocument> GetDocumentByIdAsync(string key);

        /// <summary>
        /// Returns the document with the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The document.</returns>
        ECADocument GetDocumentById(string key);

        /// <summary>
        /// Returns the document with the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The document.</returns>
        Task<ECADocument> GetDocumentByIdAsync(DocumentKey key);

        /// <summary>
        /// Returns the document with the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The document.</returns>
        ECADocument GetDocumentById(DocumentKey key);

        /// <summary>
        /// Indexes the given objects to documents for the index.
        /// </summary>
        /// <typeparam name="T">The class type of the objects to convert to documents.</typeparam>
        /// <param name="documents">The objects that will be documents.</param>
        /// <returns>The response.</returns>
        Task<DocumentIndexResponse> HandleDocumentsAsync<T>(List<T> documents) where T : class;

        /// <summary>
        /// Indexes the given objects to documents for the index.
        /// </summary>
        /// <typeparam name="T">The class type of the objects to convert to documents.</typeparam>
        /// <param name="documents">The objects that will be documents.</param>
        /// <returns>The response.</returns>
        DocumentIndexResponse HandleDocuments<T>(List<T> documents) where T : class;

        /// <summary>
        /// Performs a full text search against the index with the given parameters.
        /// </summary>
        /// <param name="searchParameters">The search parameters.</param>
        /// <param name="allowedDocumentKeys">The documents keys that can be searched on.</param>
        /// <returns>The search results.</returns>
        Task<DocumentSearchResponse<ECADocument>> SearchAsync(ECASearchParameters searchParameters, List<DocumentKey> allowedDocumentKeys);

        /// <summary>
        /// Performs a full text search against the index with the given parameters.
        /// </summary>
        /// <param name="suggestionParameters">The suggestion parameters.</param>
        /// <param name="allowedDocumentKeys">The documents keys that can be searched on.</param>
        /// <returns>The search results.</returns>
        DocumentSearchResponse<ECADocument> Search(ECASearchParameters searchParameters, List<DocumentKey> allowedDocumentKeys);

        /// <summary>
        /// Returns suggested search values given the parameters.
        /// </summary>
        /// <param name="suggestionParameters">The suggestion parameters.</param>
        /// <param name="allowedDocumentKeys">The document keys the search is allowed to include.</param>
        /// <returns>The suggested search.</returns>
        DocumentSuggestResponse<ECADocument> GetSuggestions(ECASuggestionParameters suggestionParameters, List<DocumentKey> allowedDocumentKeys);

        /// <summary>
        /// Returns suggested search values given the parameters.
        /// </summary>
        /// <param name="suggestionParameters">The suggestion parameters.</param>
        /// <param name="allowedDocumentKeys">The document keys the search is allowed to include.</param>
        /// <returns>The suggested search.</returns>
        Task<DocumentSuggestResponse<ECADocument>> GetSuggestionsAsync(ECASuggestionParameters suggestionParameters, List<DocumentKey> allowedDocumentKeys);

        /// <summary>
        /// Returns the names of the document fields.
        /// </summary>
        /// <returns>The names of the document fields.</returns>
        IList<string> GetDocumentFieldNames();
    }
    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IIndexService))]
    public abstract class IndexServiceContract : IIndexService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CreateIndex<T>() where T : class
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task CreateIndexAsync<T>() where T : class
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexName"></param>
        public void DeleteIndex(string indexName)
        {
            Contract.Requires(indexName != null, "The index name must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public Task DeleteIndexAsync(string indexName)
        {
            Contract.Requires(indexName != null, "The index name must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ECADocument GetDocumentById(DocumentKey key)
        {
            Contract.Requires(key != null, "The document key must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ECADocument GetDocumentById(string key)
        {
            Contract.Requires(key != null, "The document key must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<ECADocument> GetDocumentByIdAsync(DocumentKey key)
        {
            Contract.Requires(key != null, "The document key must not be null.");
            return Task.FromResult<ECADocument>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<ECADocument> GetDocumentByIdAsync(string key)
        {
            Contract.Requires(key != null, "The document key must not be null.");
            return Task.FromResult<ECADocument>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IDocumentConfiguration GetDocumentConfiguration<T>()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<string> GetDocumentFieldNames()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="suggestionParameters"></param>
        /// <returns></returns>
        public DocumentSuggestResponse<ECADocument> GetSuggestions(ECASuggestionParameters suggestionParameters, List<DocumentKey> allowedDocumentKeys)
        {
            Contract.Requires(suggestionParameters != null, "The suggestion parameters must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="suggestionParameters"></param>
        /// <returns></returns>
        public Task<DocumentSuggestResponse<ECADocument>> GetSuggestionsAsync(ECASuggestionParameters suggestionParameters, List<DocumentKey> allowedDocumentKeys)
        {
            Contract.Requires(suggestionParameters != null, "The suggestion parameters must not be null.");
            return Task.FromResult<DocumentSuggestResponse<ECADocument>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documents"></param>
        /// <returns></returns>
        public DocumentIndexResponse HandleDocuments<T>(List<T> documents) where T : class
        {
            Contract.Requires(documents != null, "The documents must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documents"></param>
        /// <returns></returns>
        public Task<DocumentIndexResponse> HandleDocumentsAsync<T>(List<T> documents) where T : class
        {
            Contract.Requires(documents != null, "The documents must not be null.");
            return Task.FromResult<DocumentIndexResponse>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="allowedDocumentKeys"></param>
        /// <returns></returns>
        public DocumentSearchResponse<ECADocument> Search(ECASearchParameters searchParameters, List<DocumentKey> allowedDocumentKeys)
        {
            Contract.Requires(searchParameters != null, "The search parameters must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="allowedDocumentKeys"></param>
        /// <returns></returns>
        public Task<DocumentSearchResponse<ECADocument>> SearchAsync(ECASearchParameters searchParameters, List<DocumentKey> allowedDocumentKeys)
        {
            Contract.Requires(searchParameters != null, "The search parameters must not be null.");
            return Task.FromResult<DocumentSearchResponse<ECADocument>>(null);
        }
    }
}