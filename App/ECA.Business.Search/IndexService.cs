using Hyak.Common;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public class IndexService : IDisposable, IIndexService
    {
        private SearchServiceClient searchClient;

        public IndexService(SearchServiceClient searchClient)
        {
            Contract.Requires(searchClient != null, "The search client must not be null.");
            this.searchClient = searchClient;
        }


        #region Exists index

        public bool Exists(DocumentType documentType)
        {
            return this.searchClient.Indexes.Exists(documentType.IndexName);
        }

        public async Task<bool> ExistsAsync(DocumentType documentType)
        {
            return await this.searchClient.Indexes.ExistsAsync(documentType.IndexName);
        }

        #endregion

        #region Create Index

        public void CreateIndex(IDocumentable documentable)
        {
            var ecaDocument = new ECADocument(documentable);
            var indexName = ecaDocument.DocumentType.IndexName;
            if (!this.searchClient.Indexes.Exists(indexName))
            {
                this.searchClient.Indexes.CreateOrUpdate(ecaDocument.GetIndex());
            }
        }

        public async Task CreateIndexAsync(IDocumentable documentable)
        {
            var ecaDocument = new ECADocument(documentable);
            var indexName = ecaDocument.DocumentType.IndexName;
            if (!await this.searchClient.Indexes.ExistsAsync(indexName))
            {
                await this.searchClient.Indexes.CreateOrUpdateAsync(ecaDocument.GetIndex());
            }
        }

        #endregion

        #region Delete Index

        public void DeleteIndex(DocumentType documentType)
        {
            var indexName = documentType.IndexName;
            if (this.searchClient.Indexes.Exists(indexName))
            {
                this.searchClient.Indexes.Delete(indexName);
            }
        }

        public async Task DeleteIndexAsync(DocumentType documentType)
        {
            var indexName = documentType.IndexName;
            if (await this.searchClient.Indexes.ExistsAsync(indexName))
            {
                await this.searchClient.Indexes.DeleteAsync(indexName);
            }
        }
        #endregion

        #region Handle Documents

        public List<DocumentIndexResponse> HandleDocuments(List<IDocumentable> documents)
        {
            var responses = new List<DocumentIndexResponse>();
            foreach (var groupedDocument in GetGroupedDocuments(documents))
            {
                var indexClient = GetClientByDocumentType(groupedDocument.DocumentType);
                var indexBatch = GetIndexBatch(groupedDocument);
                var response = indexClient.Documents.Index(indexBatch);
                responses.Add(response);
            }
            return responses;
        }

        public async Task<List<DocumentIndexResponse>> HandleDocumentsAsync(List<IDocumentable> documents)
        {
            var responses = new List<DocumentIndexResponse>();
            foreach (var groupedDocument in GetGroupedDocuments(documents))
            {
                var indexClient = GetClientByDocumentType(groupedDocument.DocumentType);
                var indexBatch = GetIndexBatch(groupedDocument);
                var response = await indexClient.Documents.IndexAsync(indexBatch);
                responses.Add(response);
            }
            return responses;
        }

        private IndexBatch GetIndexBatch(GroupedDocument groupedDocument)
        {
            var documentType = groupedDocument.DocumentType;
            var actions = new List<IndexAction>();
            foreach (var document in groupedDocument.Documents)
            {
                var ecaDocument = new ECADocument(document);
                var indexAction = new IndexAction(IndexActionType.MergeOrUpload, ecaDocument);
                actions.Add(indexAction);
            }
            var indexBatch = new IndexBatch(actions);
            return indexBatch;
        }

        private List<GroupedDocument> GetGroupedDocuments(List<IDocumentable> documents)
        {
            var groupedDocuments = from document in documents
                                   group document by document.GetDocumentType() into documentTypes
                                   select new GroupedDocument
                                   {
                                       DocumentType = documentTypes.Key,
                                       Documents = documentTypes.Select(x => x)
                                   };
            return groupedDocuments.ToList();
        }

        #endregion

        //#region Stats

        //public IndexGetStatisticsResponse GetStats(DocumentType documentType)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<IndexGetStatisticsResponse> GetStatsAsync(DocumentType documentType)
        //{
        //    var indexName = documentType.IndexName;
        //    var stats = await this.searchClient.Indexes.GetStatisticsAsync(indexName);
        //    return stats;
        //}

        //#endregion

        //#region Get Doc by Id

        //public Document GetDocumentById(DocumentType documentType, int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<Document> GetDocumentByIdAsync(DocumentType documentType, int id)
        //{
        //    var indexName = documentType.IndexName;
        //    var client = this.searchClient.Indexes.GetClient(indexName);
        //    var documentKey = new DocumentKey(documentType, id);
        //    var doc = await client.Documents.GetAsync(documentKey.ToString());
        //    return doc.Document;
        //}

        //#endregion

        #region Search

        public DocumentSearchResponse Search(DocumentType documentType, string search, List<DocumentKey> allowedDocumentKeys)
        {
            var client = GetClientByDocumentType(documentType);
            var parameters = GetSearchParameters(documentType, search, allowedDocumentKeys);
            return client.Documents.Search(search, parameters);
        }

        public async Task<DocumentSearchResponse> SearchAsync(DocumentType documentType, string search, List<DocumentKey> allowedDocumentKeys)
        {
            var client = GetClientByDocumentType(documentType);
            var parameters = GetSearchParameters(documentType, search, allowedDocumentKeys);
            return await client.Documents.SearchAsync(search, parameters);
        }

        private SearchIndexClient GetClientByDocumentType(DocumentType documentType)
        {
            var indexName = documentType.IndexName;
            var client = this.searchClient.Indexes.GetClient(indexName);
            return client;
        }

        private SearchParameters GetSearchParameters(DocumentType documentType, string search, List<DocumentKey> allowedDocumentKeys)
        {
            return new SearchParameters
            {

            };
        }

        #endregion


        #region IDispose

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.searchClient != null)
                {
                    this.searchClient.Dispose();
                    this.searchClient = null;
                }
            }
        }


        #endregion

        private class GroupedDocument
        {
            public DocumentType DocumentType { get; set; }

            public IEnumerable<IDocumentable> Documents { get; set; }
        }
    }


}

