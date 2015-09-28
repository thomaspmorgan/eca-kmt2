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

        public IndexService(SearchServiceClient searchClient, List<IDocumentConfiguration> documentConfigurations = null)
        {
            Contract.Requires(searchClient != null, "The search client must not be null.");
            this.searchClient = searchClient;
            if (documentConfigurations == null)
            {
                this.Configurations = new List<IDocumentConfiguration>();
            }
            else
            {
                var configurationsWithoutDocumentType = documentConfigurations.Where(x => x.GetDocumentType() == null).ToList();
                if (configurationsWithoutDocumentType.Count > 0)
                {
                    throw new NotSupportedException(String.Format("The following configurations do not define a document type:  {0}.",
                        String.Join(", ",
                        configurationsWithoutDocumentType.Select(x => x.GetType()).ToList()
                        )));
                }

                var distinctConfigurations = from config in documentConfigurations
                                             group config by config.GetDocumentType() into g
                                             select new
                                             {
                                                 DocumentType = g.Key,
                                                 Count = g.Count(),
                                                 Config = g.Select(x => x)
                                             };
                var invalidConfigurations = distinctConfigurations.Where(x => x.Count > 1).ToList();
                if (invalidConfigurations.Count > 0)
                {
                    throw new NotSupportedException(String.Format("The following document types are configured more than once:  {0}.",
                        String.Join(", ",
                        invalidConfigurations.Select(x => x.DocumentType).ToList()
                        )));
                }
                this.Configurations = distinctConfigurations.Select(x => x.Config.First()).ToList();
            }
        }

        public List<IDocumentConfiguration> Configurations { get; private set; }

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

        public Index GetIndex(IDocumentConfiguration configuration)
        {
            Contract.Requires(configuration != null, "The configuration must not be null.");
            var index = new Index
            {
                Name = configuration.GetDocumentType().IndexName,
            };            
            index.Fields.Add(new Field
            {
                IsKey = true,
                Name = ECADocument.ID_KEY,
                Type = DataType.String
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = ECADocument.NAME_KEY,
                Type = DataType.String,
                IsSearchable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = ECADocument.DOCUMENT_TYPE_KEY,
                Type = DataType.String,
                IsSearchable = true,
                IsFacetable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = ECADocument.DESCRIPTION_KEY,
                Type = DataType.String,
                IsSearchable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = ECADocument.FOCI_KEY,
                Type = DataType.Collection(DataType.String),
                IsSearchable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = ECADocument.GOALS_KEY,
                Type = DataType.Collection(DataType.String),
                IsSearchable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = ECADocument.OBJECTIVES_KEY,
                Type = DataType.Collection(DataType.String),
                IsSearchable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = ECADocument.THEMES_KEY,
                Type = DataType.Collection(DataType.String),
                IsSearchable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = ECADocument.POINTS_OF_CONTACT_KEY,
                Type = DataType.Collection(DataType.String),
                IsSearchable = true
            });
            return index;
        }

        public void CreateIndex<T>() where T : class
        {
            var configuration = GetDocumentConfiguration<T>();
            var index = GetIndex(configuration);
            var indexName = index.Name;
            if (!this.searchClient.Indexes.Exists(indexName))
            {
                this.searchClient.Indexes.CreateOrUpdate(index);
            }
        }

        public async Task CreateIndexAsync<T>() where T : class
        {
            var configuration = GetDocumentConfiguration<T>();
            var index = GetIndex(configuration);
            var indexName = index.Name;
            if (!(await this.searchClient.Indexes.ExistsAsync(indexName)))
            {
                await this.searchClient.Indexes.CreateOrUpdateAsync(index);
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

        public async Task<DocumentIndexResponse> HandleDocumentsAsync<T>(List<T> documents) where T : class
        {
            if (documents.Count == 0)
            {
                return null;
            }
            else
            {
                var configuration = GetDocumentConfiguration<T>();
                var indexBatch = DoHandleDocuments(documents, configuration);
                var indexClient = GetClientByDocumentType(configuration.GetDocumentType());
                var response = await indexClient.Documents.IndexAsync(indexBatch);
                return response;
            }

        }

        public DocumentIndexResponse HandleDocuments<T>(List<T> documents) where T : class
        {
            if (documents.Count == 0)
            {
                return null;
            }
            else
            {
                var configuration = GetDocumentConfiguration<T>();
                var indexBatch = DoHandleDocuments(documents, configuration);
                var indexClient = GetClientByDocumentType(configuration.GetDocumentType());
                var response = indexClient.Documents.Index(indexBatch);
                return response;
            }
        }

        private IndexBatch<ECADocument> DoHandleDocuments<T>(List<T> documents, IDocumentConfiguration configuration) where T : class
        {
            var responses = new List<DocumentIndexResponse>();
            if (configuration == null)
            {
                throw new NotSupportedException(String.Format("The configuration for the type [{0}] was not found.", typeof(T)));
            }
            var actions = new List<IndexAction>();
            var ecaDocuments = new List<ECADocument>();
            documents.ForEach(x => ecaDocuments.Add(new ECADocument<T>(configuration, x)));
            return IndexBatch.Create(ecaDocuments.Select(d => IndexAction.Create(d)));
        }

        public IDocumentConfiguration GetDocumentConfiguration<T>()
        {
            return this.Configurations.Where(x => x.IsConfigurationForType(typeof(T))).FirstOrDefault();
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

        public DocumentSearchResponse<ECADocument> Search(string search, List<DocumentKey> allowedDocumentKeys)
        {
            var client = GetClient();
            var parameters = GetSearchParameters(search, allowedDocumentKeys);
            var response = client.Documents.Search<ECADocument>(search, parameters);
            return response;
        }

        public async Task<DocumentSearchResponse<ECADocument>> SearchAsync(string search, List<DocumentKey> allowedDocumentKeys)
        {
            var client = GetClient();
            var parameters = GetSearchParameters(search, allowedDocumentKeys);
            var response = await client.Documents.SearchAsync<ECADocument>(search, parameters);
            return response;
        }

        private SearchIndexClient GetClientByDocumentType(DocumentType documentType)
        {
            return GetClient();
        }

        private SearchIndexClient GetClient()
        {
            return this.searchClient.Indexes.GetClient(DocumentType.ALL_DOCUMENTS_INDEX_NAME);
        }
        private SearchParameters GetSearchParameters(string search, List<DocumentKey> allowedDocumentKeys)
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
        
    }
}

