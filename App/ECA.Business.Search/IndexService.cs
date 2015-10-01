﻿using ECA.Core.DynamicLinq;
using Hyak.Common;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// An IndexService is used to handle indexing and searching documents that reside in Azure Search.  Provide
    /// a
    /// </summary>
    public class IndexService : IDisposable, IIndexService
    {
        public const string INDEX_NAME = "ecadocs";

        private SearchServiceClient searchClient;

        /// <summary>
        /// Creates a new IndexService given the Azure Search client instance and the document configurations.
        /// 
        /// The List of IDocumentConfigurations will be used to create azure search compatible documents from objects
        /// that are classes.
        /// </summary>
        /// <param name="searchClient">The azure search service client instance.</param>
        /// <param name="documentConfigurations">The document configurations for classes that will be indexed.</param>
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
                var distinctConfigurations = documentConfigurations
                    .GroupBy(x => x.GetType())
                    .Select(x => x.First())
                    .ToList();
                var configurationsWithoutDocumentType = distinctConfigurations.Where(x => x.GetDocumentTypeId() == Guid.Empty).ToList();
                if (configurationsWithoutDocumentType.Count > 0)
                {
                    throw new NotSupportedException(String.Format("The following configurations do not define a document type:  {0}.",
                        String.Join(", ",
                        configurationsWithoutDocumentType.Select(x => x.GetType()).ToList()
                        )));
                }

                var distinctDocumentTypeIdConfigurations = from config in distinctConfigurations
                                                           group config by config.GetDocumentTypeId() into g
                                                           select new
                                                           {
                                                               DocumentTypeId = g.Key,
                                                               Count = g.Count()
                                                           };
                var invalidDocumentTypeIdConfigurations = distinctDocumentTypeIdConfigurations.Where(x => x.Count > 1).ToList();
                if (invalidDocumentTypeIdConfigurations.Count > 0)
                {
                    throw new NotSupportedException(String.Format("The document type ids {0} are not unique.",
                        String.Join(", ", invalidDocumentTypeIdConfigurations.Select(x => x.DocumentTypeId).ToList())));

                }

                var distinctDocumentTypeNameConfigurations = from config in documentConfigurations
                                                             group config by config.GetDocumentTypeName() into g
                                                             select new
                                                             {
                                                                 DocumentTypeName = g.Key,
                                                                 Count = g.Count(),
                                                             };
                var invalidDocumentTypeNameConfigurations = distinctDocumentTypeNameConfigurations.Where(x => x.Count > 1).ToList();
                if (invalidDocumentTypeNameConfigurations.Count > 0)
                {
                    throw new NotSupportedException(String.Format("The document type names {0} are not unique.",
                        String.Join(", ", invalidDocumentTypeNameConfigurations.Select(x => x.DocumentTypeName).ToList())));
                }
                this.Configurations = distinctConfigurations;
            }
        }

        public List<IDocumentConfiguration> Configurations { get; private set; }

        #region Exists index

        /// <summary>
        /// Returns true if the document type index exists.
        /// </summary>
        /// <param name="documentType">The document type.</param>
        /// <returns>True if the document type index exists, otherwise false.</returns>
        public bool Exists(string indexName)
        {
            return this.searchClient.Indexes.Exists(indexName);
        }

        /// <summary>
        /// Returns true if the document type index exists.
        /// </summary>
        /// <param name="indexName">The index name.</param>
        /// <returns>True if the document type index exists, otherwise false.</returns>
        public async Task<bool> ExistsAsync(string indexName)
        {
            return await this.searchClient.Indexes.ExistsAsync(indexName);
        }

        #endregion

        #region Create Index

        /// <summary>
        /// Creates an Azure Search index schema given the document configuration.
        /// </summary>
        /// <param name="configuration">The document configuration.</param>
        /// <returns>The azure search index.</returns>
        public Index GetIndex(IDocumentConfiguration configuration)
        {
            Contract.Requires(configuration != null, "The configuration must not be null.");
            var index = new Index
            {
                Name = INDEX_NAME,
            };
            index.Fields.Add(new Field
            {
                IsKey = true,
                Name = PropertyHelper.GetPropertyName<ECADocument>(x => x.Id),
                Type = DataType.String
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = PropertyHelper.GetPropertyName<ECADocument>(x => x.Name),
                Type = DataType.String,
                IsSearchable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = PropertyHelper.GetPropertyName<ECADocument>(x => x.DocumentTypeName),
                Type = DataType.String,
                IsSearchable = true,
                IsFacetable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = PropertyHelper.GetPropertyName<ECADocument>(x => x.DocumentTypeId),
                Type = DataType.String,
                IsSearchable = false,
                IsRetrievable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = PropertyHelper.GetPropertyName<ECADocument>(x => x.Description),
                Type = DataType.String,
                IsSearchable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = PropertyHelper.GetPropertyName<ECADocument>(x => x.Foci),
                Type = DataType.Collection(DataType.String),
                IsSearchable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = PropertyHelper.GetPropertyName<ECADocument>(x => x.Goals),
                Type = DataType.Collection(DataType.String),
                IsSearchable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = PropertyHelper.GetPropertyName<ECADocument>(x => x.Objectives),
                Type = DataType.Collection(DataType.String),
                IsSearchable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = PropertyHelper.GetPropertyName<ECADocument>(x => x.Themes),
                Type = DataType.Collection(DataType.String),
                IsSearchable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = PropertyHelper.GetPropertyName<ECADocument>(x => x.PointsOfContact),
                Type = DataType.Collection(DataType.String),
                IsSearchable = true
            });
            index.Fields.Add(new Field
            {
                IsKey = false,
                Name = PropertyHelper.GetPropertyName<ECADocument>(x => x.OfficeSymbol),
                Type = DataType.String,
                IsSearchable = true
            });
            foreach (var field in index.Fields)
            {
                field.Name = ToCamelCase(field.Name);
            }
            return index;
        }

        private string ToCamelCase(string propertyName)
        {
            Contract.Requires(propertyName != null && propertyName.Length > 0, "The property name is invalid.");
            return propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1);
        }


        /// <summary>
        /// Creates an index for the given type T to house documents in.
        /// </summary>
        /// <typeparam name="T">The type T for the documents.</typeparam>
        public void CreateIndex<T>() where T : class
        {
            var configuration = GetDocumentConfiguration<T>();
            var index = GetIndex(configuration);
            var indexName = index.Name;
            this.searchClient.Indexes.CreateOrUpdate(index);
        }

        /// <summary>
        /// Creates an index for the given type T to house documents in.
        /// </summary>
        /// <typeparam name="T">The type T for the documents.</typeparam>
        /// <returns>The task.</returns>
        public async Task CreateIndexAsync<T>() where T : class
        {
            var configuration = GetDocumentConfiguration<T>();
            var index = GetIndex(configuration);
            var indexName = index.Name;
            await this.searchClient.Indexes.CreateOrUpdateAsync(index);
        }

        #endregion

        #region Delete Index

        /// <summary>
        /// Deletes the index for the given document type, if it exists.
        /// </summary>
        /// <param name="documentType">The document type.</param>
        public void DeleteIndex(string indexName)
        {
            if (this.searchClient.Indexes.Exists(indexName))
            {
                this.searchClient.Indexes.Delete(indexName);
            }
        }

        /// <summary>
        /// Deletes the index for the given document type, if it exists.
        /// </summary>
        /// <param name="documentType">The document type.</param>
        /// <returns>The task.</returns>
        public async Task DeleteIndexAsync(string indexName)
        {
            if (await this.searchClient.Indexes.ExistsAsync(indexName))
            {
                await this.searchClient.Indexes.DeleteAsync(indexName);
            }
        }
        #endregion

        #region Handle Documents

        /// <summary>
        /// Indexes the given documents to Azure Search.
        /// </summary>
        /// <typeparam name="T">The type of documents to index.</typeparam>
        /// <param name="documents">The documents.</param>
        /// <returns>The indexing response, or null if no documents to index.</returns>
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
                var indexClient = GetClient();
                var response = await indexClient.Documents.IndexAsync(indexBatch);
                return response;
            }
        }

        /// <summary>
        /// Indexes the given documents to Azure Search.
        /// </summary>
        /// <typeparam name="T">The type of documents to index.</typeparam>
        /// <param name="documents">The documents.</param>
        /// <returns>The indexing response, or null if no documents to index.</returns>
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
                var indexClient = GetClient();
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
            Contract.Assert(configuration.IsConfigurationForType(typeof(T)), 
                String.Format("The IDocumentConfiguration {0} is not valid for the type {1}.", configuration.GetType(), typeof(T)));
            var actions = new List<IndexAction>();
            var ecaDocuments = new List<ECADocument>();
            documents.ForEach(x =>
            {
                var ecaDocument = new ECADocument<T>(configuration, x);
                Contract.Assert(!String.IsNullOrWhiteSpace(ecaDocument.Id), String.Format("The document for object of type {0} must have a valid id.", typeof(T)));
                Contract.Assert(!String.IsNullOrWhiteSpace(ecaDocument.DocumentTypeId), String.Format("The document for object of type {0} must have a valid document type id.", typeof(T)));
                Contract.Assert(!String.IsNullOrWhiteSpace(ecaDocument.DocumentTypeName), String.Format("The document for object of type {0} must have a valid document type name.", typeof(T)));
                ecaDocuments.Add(ecaDocument);
            });
            return IndexBatch.Create(ecaDocuments.Select(d => IndexAction.Create(IndexActionType.MergeOrUpload, d)));
        }

        /// <summary>
        /// Returns the document configuration for the type T or null if it does not exist.
        /// </summary>
        /// <typeparam name="T">The type T to retrieve a document configuration for.</typeparam>
        /// <returns>The document configuration or null.</returns>
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

        #region Get Doc by Id

        /// <summary>
        /// Returns the document with the given key.
        /// </summary>
        /// <param name="key">The document key.</param>
        /// <returns>The document.</returns>
        public ECADocument GetDocumentById(string key)
        {
            return GetDocumentById(new DocumentKey(key));
        }

        /// <summary>
        /// Returns the document with the given key.
        /// </summary>
        /// <param name="key">The document key.</param>
        /// <returns>The document.</returns>
        public Task<ECADocument> GetDocumentByIdAsync(string key)
        {
            return GetDocumentByIdAsync(new DocumentKey(key));
        }

        /// <summary>
        /// Returns the document with the given key.
        /// </summary>
        /// <param name="key">The document key.</param>
        /// <returns>The document.</returns>
        public ECADocument GetDocumentById(DocumentKey key)
        {
            var client = GetClient();
            var doc = client.Documents.Get<ECADocument>(key.ToString());
            return doc.Document;
        }

        /// <summary>
        /// Returns the document with the given key.
        /// </summary>
        /// <param name="key">The document key.</param>
        /// <returns>The document.</returns>
        public async Task<ECADocument> GetDocumentByIdAsync(DocumentKey key)
        {
            var client = GetClient();
            var doc = await client.Documents.GetAsync<ECADocument>(key.ToString());
            return doc.Document;
        }

        #endregion

        #region Search

        /// <summary>
        /// Performs a search against Azure Search using the parameters and allowed document keys.
        /// </summary>
        /// <param name="ecaSearchParameters">The eca search parameters.</param>
        /// <param name="allowedDocumentKeys">The document keys the search is allowed to include.</param>
        /// <returns>The search result.</returns>
        public DocumentSearchResponse<ECADocument> Search(ECASearchParameters ecaSearchParameters, List<DocumentKey> allowedDocumentKeys)
        {
            var client = GetClient();
            var parameters = GetSearchParameters(ecaSearchParameters, allowedDocumentKeys);
            var response = client.Documents.Search<ECADocument>(ecaSearchParameters.SearchTerm, parameters);
            return response;
        }

        /// <summary>
        /// Performs a search against Azure Search using the parameters and allowed document keys.
        /// </summary>
        /// <param name="ecaSearchParameters">The eca search parameters.</param>
        /// <param name="allowedDocumentKeys">The document keys the search is allowed to include.</param>
        /// <returns>The search result.</returns>
        public async Task<DocumentSearchResponse<ECADocument>> SearchAsync(ECASearchParameters ecaSearchParameters, List<DocumentKey> allowedDocumentKeys)
        {
            var client = GetClient();
            var parameters = GetSearchParameters(ecaSearchParameters, allowedDocumentKeys);
            var response = await client.Documents.SearchAsync<ECADocument>(ecaSearchParameters.SearchTerm, parameters);
            return response;
        }

        private SearchIndexClient GetClient()
        {
            return GetClient(INDEX_NAME);
        }

        private SearchIndexClient GetClient(string indexName)
        {
            return this.searchClient.Indexes.GetClient(indexName);
        }

        /// <summary>
        /// Returns azure search parameters given the eca search parameters and all allowed document keys.
        /// </summary>
        /// <param name="ecaSearchParameters">The eca search parameters.</param>
        /// <param name="allowedDocumentKeys">The document keys that a search is allowed to include.</param>
        /// <returns>The SearchParameters instance for an azure search.</returns>
        public SearchParameters GetSearchParameters(ECASearchParameters ecaSearchParameters, List<DocumentKey> allowedDocumentKeys)
        {
            var searchParameters = new SearchParameters
            {
                Skip = ecaSearchParameters.Start,
                Top = ecaSearchParameters.Limit,
                Filter = ecaSearchParameters.Filter
            };
            if (ecaSearchParameters.Facets != null)
            {
                var distinctFacets = ecaSearchParameters.Facets.Distinct().ToList();
                searchParameters.Facets = distinctFacets;
            }
            if (ecaSearchParameters.Fields != null)
            {
                var distinctFields = ecaSearchParameters.Fields.Distinct().ToList();
                searchParameters.Select = distinctFields;
            }
            return searchParameters;
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

        /// <summary>
        /// Returns instances of all concrete classes that implement the IDocumentConfiguration in the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly to retrieve configurations from.</param>
        /// <returns>Instances of all concrete classes that implmement the IDocumentConfiguration.</returns>
        public static IEnumerable<IDocumentConfiguration> GetAllConfigurations(Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(x => typeof(IDocumentConfiguration).IsAssignableFrom(x) && !x.IsAbstract && x.IsClass && !x.IsInterface)
                .ToList();
            var configs = new List<IDocumentConfiguration>();
            types.ForEach(x =>
            {
                configs.Add((IDocumentConfiguration)Activator.CreateInstance(x));
            });
            return configs;
        }
    }
}

