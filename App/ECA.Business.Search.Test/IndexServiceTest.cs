using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.Azure.Search.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.Azure;
using System.Reflection;
using ECA.Core.DynamicLinq;
using ECA.Business.Search.Fakes;

namespace ECA.Business.Search.Test
{

    [TestClass]
    public class IndexServiceTest
    {
        private ShimSearchServiceClient searchClient;
        private IndexService service;
        private string indexName;

        [TestInitialize]
        public void TestInit()
        {
            indexName = "index";
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Exists
        [TestMethod]
        public async Task TestExists()
        {
            bool doesExist = true;
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var index = new StubIIndexOperations
                {
                    ExistsAsyncStringCancellationToken = (indexName, cancelToken) =>
                    {
                        return Task.FromResult<bool>(doesExist);
                    },
                };
                searchClient.IndexesGet = () =>
                {
                    return index;
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsAsyncIIndexOperationsString = (operations, indexName) =>
                {
                    Assert.AreEqual(indexName, indexName);
                    return Task.FromResult<bool>(doesExist);
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsIIndexOperationsString = (operations, indexName) =>
                {
                    Assert.AreEqual(indexName, indexName);
                    return doesExist;
                };

                service = new IndexService(indexName, searchClient.Instance);
                Assert.AreEqual(doesExist, service.Exists(indexName));
                Assert.AreEqual(doesExist, await service.ExistsAsync(indexName));
            }
        }

        [TestMethod]
        public async Task TestExists_IndexDoesNotExist()
        {
            bool doesExist = false;
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var index = new StubIIndexOperations
                {
                    ExistsAsyncStringCancellationToken = (indexName, cancelToken) =>
                    {
                        return Task.FromResult<bool>(doesExist);
                    },
                };
                searchClient.IndexesGet = () =>
                {
                    return index;
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsAsyncIIndexOperationsString = (operations, indexName) =>
                {
                    Assert.AreEqual(indexName, indexName);
                    return Task.FromResult<bool>(doesExist);
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsIIndexOperationsString = (operations, indexName) =>
                {
                    Assert.AreEqual(indexName, indexName);
                    return doesExist;
                };

                service = new IndexService(indexName, searchClient.Instance);
                Assert.AreEqual(doesExist, service.Exists(indexName));
                Assert.AreEqual(doesExist, await service.ExistsAsync(indexName));
            }
        }

        #endregion

        #region GetDocumentById
        [TestMethod]
        public async Task TestGetDocumentById_String()
        {
            var documentTypeId = Guid.NewGuid();
            var key = new DocumentKey(documentTypeId, 1);
            var getCalled = false;
            var getAsyncCalled = false;
            var documentGetResponse = new DocumentGetResponse<ECADocument>();
            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {
                    DocumentsGet = () =>
                    {
                        return documentOperations;
                    }
                };
                var indexOperations = new StubIIndexOperations
                {
                    GetClientString = (name) =>
                    {
                        return searchIndexClient;
                    }
                };

                searchClient = new ShimSearchServiceClient();

                searchClient.IndexesGet = () =>
                {
                    return indexOperations;
                };

                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.GetAsyncOf1IDocumentOperationsString<ECADocument>((operations, id) =>
                {
                    Assert.AreEqual(key.ToString(), id);
                    getAsyncCalled = true;
                    return Task.FromResult<DocumentGetResponse<ECADocument>>(documentGetResponse);
                });
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.GetOf1IDocumentOperationsString<ECADocument>((operations, id) =>
                {
                    Assert.AreEqual(key.ToString(), id);
                    getCalled = true;
                    return documentGetResponse;
                });


                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(getAsyncCalled);
                Assert.IsFalse(getCalled);

                service.GetDocumentById(key.ToString());
                await service.GetDocumentByIdAsync(key.ToString());
                Assert.IsTrue(getAsyncCalled);
                Assert.IsTrue(getCalled);
            }
        }


        [TestMethod]
        public async Task TestGetDocumentById_DocumentKey()
        {
            var documentTypeId = Guid.NewGuid();
            var key = new DocumentKey(documentTypeId, 1);
            var getCalled = false;
            var getAsyncCalled = false;
            var documentGetResponse = new DocumentGetResponse<ECADocument>();
            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {
                    DocumentsGet = () =>
                    {
                        return documentOperations;
                    }
                };
                var indexOperations = new StubIIndexOperations
                {
                    GetClientString = (name) =>
                    {
                        return searchIndexClient;
                    }
                };

                searchClient = new ShimSearchServiceClient();

                searchClient.IndexesGet = () =>
                {
                    return indexOperations;
                };

                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.GetAsyncOf1IDocumentOperationsString<ECADocument>((operations, id) =>
                {
                    getAsyncCalled = true;
                    Assert.AreEqual(key.ToString(), id);
                    return Task.FromResult<DocumentGetResponse<ECADocument>>(documentGetResponse);
                });
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.GetOf1IDocumentOperationsString<ECADocument>((operations, id) =>
                {
                    getCalled = true;
                    Assert.AreEqual(key.ToString(), id);
                    return documentGetResponse;
                });
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(getAsyncCalled);
                Assert.IsFalse(getCalled);

                service.GetDocumentById(key);
                await service.GetDocumentByIdAsync(key);
                Assert.IsTrue(getAsyncCalled);
                Assert.IsTrue(getCalled);
            }
        }
        #endregion

        #region Create
        [TestMethod]
        public async Task TestCreateIndex_DocumentIndexDoesNotExist()
        {
            var calledCreate = false;
            var calledCreateAsync = false;
            var indexDefinitionResponse = new IndexDefinitionResponse();
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var index = new StubIIndexOperations
                {
                };
                searchClient.IndexesGet = () =>
                {
                    return index;
                };

                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.CreateOrUpdateAsyncIIndexOperationsIndex = (operations, idx) =>
                {
                    Assert.AreEqual(this.indexName, idx.Name);
                    calledCreateAsync = true;
                    return Task.FromResult<IndexDefinitionResponse>(indexDefinitionResponse);
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.CreateOrUpdateIIndexOperationsIndex = (operations, idx) =>
                {
                    Assert.AreEqual(this.indexName, idx.Name);
                    calledCreate = true;
                    return indexDefinitionResponse;
                };

                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(calledCreate);
                Assert.IsFalse(calledCreateAsync);
                service.CreateIndex<TestDocument>();
                await service.CreateIndexAsync<TestDocument>();
                Assert.IsTrue(calledCreate);
                Assert.IsTrue(calledCreateAsync);
            }
        }

        #endregion

        #region Delete
        [TestMethod]
        public async Task TestDeleteIndex_DocumentIndexDoesNotExist()
        {
            bool doesExist = false;
            bool calledDoesExist = false;
            bool calledDoesExistAsync = false;
            var calledDelete = false;
            var calledDeleteAsync = false;
            var response = new AzureOperationResponse();
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var index = new StubIIndexOperations
                {
                };
                searchClient.IndexesGet = () =>
                {
                    return index;
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsAsyncIIndexOperationsString = (operations, indexName) =>
                {
                    Assert.AreEqual(this.indexName, indexName);
                    calledDoesExistAsync = true;
                    return Task.FromResult<bool>(doesExist);
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsIIndexOperationsString = (operations, indexName) =>
                {
                    Assert.AreEqual(this.indexName, indexName);
                    calledDoesExist = true;
                    return doesExist;
                };

                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(calledDelete);
                Assert.IsFalse(calledDeleteAsync);
                Assert.IsFalse(calledDoesExist);
                Assert.IsFalse(calledDoesExistAsync);
                var testDocument = new TestDocument();
                service.DeleteIndex(this.indexName);
                await service.DeleteIndexAsync(this.indexName);
                Assert.IsFalse(calledDelete);
                Assert.IsFalse(calledDeleteAsync);
                Assert.IsTrue(calledDoesExist);
                Assert.IsTrue(calledDoesExistAsync);
            }
        }

        [TestMethod]
        public async Task TestDeleteIndex_DocumentIndexExists()
        {
            bool doesExist = true;
            bool calledDoesExist = false;
            bool calledDoesExistAsync = false;
            var calledDelete = false;
            var calledDeleteAsync = false;
            var response = new AzureOperationResponse();
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var index = new StubIIndexOperations
                {

                };
                searchClient.IndexesGet = () =>
                {
                    return index;
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsAsyncIIndexOperationsString = (operations, indexName) =>
                {
                    Assert.AreEqual(this.indexName, indexName);
                    calledDoesExistAsync = true;
                    return Task.FromResult<bool>(doesExist);
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsIIndexOperationsString = (operations, indexName) =>
                {
                    Assert.AreEqual(this.indexName, indexName);
                    calledDoesExist = true;
                    return doesExist;
                };

                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.DeleteAsyncIIndexOperationsString = (operations, indexName) =>
                {
                    Assert.AreEqual(this.indexName, indexName);
                    calledDeleteAsync = true;
                    return Task.FromResult<AzureOperationResponse>(response);
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.DeleteIIndexOperationsString = (operations, indexName) =>
                {
                    Assert.AreEqual(this.indexName, indexName);
                    calledDelete = true;
                    return response;
                };
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(calledDelete);
                Assert.IsFalse(calledDeleteAsync);
                Assert.IsFalse(calledDoesExist);
                Assert.IsFalse(calledDoesExistAsync);
                var testDocument = new TestDocument();
                service.DeleteIndex(this.indexName);
                await service.DeleteIndexAsync(this.indexName);
                Assert.IsTrue(calledDelete);
                Assert.IsTrue(calledDeleteAsync);
                Assert.IsTrue(calledDoesExist);
                Assert.IsTrue(calledDoesExistAsync);
            }
        }
        #endregion

        #region Add Or Update Documents
        [TestMethod]
        public async Task TestAddOrUpdate()
        {
            var indexCalled = false;
            var indexAsyncCalled = false;
            var documentIndexResponse = new DocumentIndexResponse();
            var configuration = new TestDocumentConfiguration(true);
            var testDocument = new TestDocument();
            var documents = new List<TestDocument>();
            documents.Add(new TestDocument());
            Action<IndexBatch<ECADocument>, TestDocument> tester = (testBatch, expectedDocument) =>
            {
                Assert.AreEqual(1, testBatch.Actions.Count());
                var firstAction = testBatch.Actions.First();
                Assert.AreEqual(IndexActionType.MergeOrUpload, firstAction.ActionType);
                Assert.AreEqual(new DocumentKey(configuration.GetDocumentTypeId(), expectedDocument.Id).ToString(), firstAction.Document.Id);
            };

            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {
                    DocumentsGet = () =>
                    {
                        return documentOperations;
                    }
                };
                var indexOperations = new StubIIndexOperations
                {
                    GetClientString = (name) =>
                    {
                        return searchIndexClient;
                    }
                };

                searchClient = new ShimSearchServiceClient();

                searchClient.IndexesGet = () =>
                {
                    return indexOperations;
                };

                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexCalled = true;
                    tester(batch, testDocument);
                    return documentIndexResponse;
                });
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexAsyncOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexAsyncCalled = true;
                    tester(batch, testDocument);
                    return Task.FromResult<DocumentIndexResponse>(documentIndexResponse);
                });

                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                Assert.IsFalse(indexCalled);
                Assert.IsFalse(indexAsyncCalled);
                var docResponse = service.AddOrUpdate(documents);
                var docResponseAsync = await service.AddOrUpdateAsync(documents);

                Assert.IsNotNull(docResponse);
                Assert.IsNotNull(docResponseAsync);
                Assert.IsTrue(indexCalled);
                Assert.IsTrue(indexAsyncCalled);
            }
        }

        [TestMethod]
        public async Task TestAddOrUpdate_DocumentIdIsNull()
        {
            var indexCalled = false;
            var indexAsyncCalled = false;
            var documentIndexResponse = new DocumentIndexResponse();
            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {
                    DocumentsGet = () =>
                    {
                        return documentOperations;
                    }
                };
                var indexOperations = new StubIIndexOperations
                {
                    GetClientString = (name) =>
                    {
                        return searchIndexClient;
                    }
                };

                searchClient = new ShimSearchServiceClient();

                searchClient.IndexesGet = () =>
                {
                    return indexOperations;
                };

                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexCalled = true;
                    return documentIndexResponse;
                });
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexAsyncOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexAsyncCalled = true;
                    return Task.FromResult<DocumentIndexResponse>(documentIndexResponse);
                });
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var testDocument = new TestDocument();
                var documents = new List<TestDocument>();
                documents.Add(new TestDocument());
                Assert.IsFalse(indexCalled);
                Assert.IsFalse(indexAsyncCalled);
                var docResponse = service.AddOrUpdate(documents);
                var docResponseAsync = await service.AddOrUpdateAsync(documents);

                Assert.IsNotNull(docResponse);
                Assert.IsNotNull(docResponseAsync);
                Assert.IsTrue(indexCalled);
                Assert.IsTrue(indexAsyncCalled);
            }
        }

        [TestMethod]
        public async Task TestAddOrUpdate_NoDocumentsProvided()
        {
            var indexCalled = false;
            var indexAsyncCalled = false;
            var documentIndexResponse = new DocumentIndexResponse();
            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {
                };
                var indexOperations = new StubIIndexOperations
                {
                };

                searchClient = new ShimSearchServiceClient();


                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var testDocument = new TestDocument();
                var documents = new List<TestDocument>();
                Assert.IsFalse(indexCalled);
                Assert.IsFalse(indexAsyncCalled);
                var docResponse = service.AddOrUpdate(documents);
                var docResponseAsync = await service.AddOrUpdateAsync(documents);

                Assert.IsNull(docResponse);
                Assert.IsNull(docResponseAsync);
                Assert.IsFalse(indexCalled);
                Assert.IsFalse(indexAsyncCalled);
            }
        }

        [TestMethod]
        public async Task TestAddOrUpdate_ConfigurationNotProvided()
        {
            var documentIndexResponse = new DocumentIndexResponse();
            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {
                    DocumentsGet = () =>
                    {
                        return documentOperations;
                    }
                };
                var indexOperations = new StubIIndexOperations
                {
                    GetClientString = (name) =>
                    {
                        return searchIndexClient;
                    }
                };

                searchClient = new ShimSearchServiceClient();

                searchClient.IndexesGet = () =>
                {
                    return indexOperations;
                };


                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration>());

                var testDocument = new TestDocument();
                var documents = new List<TestDocument>();
                documents.Add(new TestDocument());

                var message = String.Format("The configuration for the type [{0}] was not found.", typeof(TestDocument));
                Func<Task> f = () =>
                {
                    return service.AddOrUpdateAsync(documents);
                };

                Action a = () => service.AddOrUpdate(documents);
                a.ShouldThrow<NotSupportedException>().WithMessage(message);
                f.ShouldThrow<NotSupportedException>().WithMessage(message);
            }
        }

        #endregion

        #region Constructor
        [TestMethod]
        public void TestConstructor_CheckIndexName()
        {
            using (ShimsContext.Create())
            {
                var searchIndexClient = new ShimSearchIndexClient
                {

                };
                searchClient = new ShimSearchServiceClient();
                var instance = new TestDocumentConfiguration(true);
                var service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { instance });
                Assert.AreEqual(this.indexName, service.IndexName);
            }
        }

        [TestMethod]
        public void TestConstructor_CheckDistinctDocumentTypeIds()
        {
            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {

                };

                searchClient = new ShimSearchServiceClient();
                var documentConfig1TypeId = Guid.NewGuid();
                var documentConfig2TypeId = documentConfig1TypeId;
                var name1 = "name1";
                var instance = new TestDocumentConfiguration(true);

                var documentConfiguration1 = new ECA.Business.Search.Fakes.StubIDocumentConfiguration
                {
                    GetDocumentTypeId = () =>
                    {
                        return instance.GetDocumentTypeId();
                    },
                    GetDocumentTypeName = () =>
                    {
                        return name1;
                    }
                };
                Action a = () => new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { documentConfiguration1, instance });

                var message = String.Format("The document type ids {0} are not unique.", instance.GetDocumentTypeId());
                a.ShouldThrow<NotSupportedException>().WithMessage(message);
            }
        }

        [TestMethod]
        public void TestConstructor_CheckDistinctDocumentTypeNames()
        {
            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {

                };
                var documentConfig1TypeId = Guid.NewGuid();
                var documentConfig2TypeId = Guid.NewGuid();
                var instance = new TestDocumentConfiguration(true);

                var documentConfiguration1 = new ECA.Business.Search.Fakes.StubIDocumentConfiguration
                {
                    GetDocumentTypeId = () =>
                    {
                        return documentConfig1TypeId;
                    },
                    GetDocumentTypeName = () =>
                    {
                        return instance.GetDocumentTypeName();
                    }
                };


                searchClient = new ShimSearchServiceClient();
                Action a = () => new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { documentConfiguration1, instance });

                var message = String.Format("The document type names {0} are not unique.", instance.GetDocumentTypeName());
                a.ShouldThrow<NotSupportedException>().WithMessage(message);
            }
        }

        [TestMethod]
        public void TestConstructor_ConfigurationDoesNotDefineDocumentType()
        {
            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {

                };

                searchClient = new ShimSearchServiceClient();
                var instance1 = new TestDocumentConfiguration();
                instance1.IsDocumentType(Guid.Empty, "hello");
                Action a = () => new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { instance1 });
                a.ShouldThrow<NotSupportedException>().WithMessage(String.Format("The following configurations do not define a document type:  {0}.", typeof(TestDocumentConfiguration)));
            }
        }

        [TestMethod]
        public void TestConstructor_DifferentConfigurations()
        {
            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {

                };

                searchClient = new ShimSearchServiceClient();
                var instance1 = new TestDocumentConfiguration(true);
                var instance2 = new OtherTestDocumentConfiguration();
                var service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { instance1, instance2 });
                Assert.AreEqual(2, service.Configurations.Count);
            }
        }
        #endregion

        #region Search
        [TestMethod]
        public void TestGetSuggestionParameters()
        {
            var searchTerm = "search";
            var preTag = "pre";
            var postTag = "post";
            var instance = new ECASuggestionParameters(searchTerm, preTag, postTag);
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var parameters = service.GetSuggestParameters(instance);
                Assert.IsTrue(parameters.UseFuzzyMatching);
                Assert.AreEqual(preTag, parameters.HighlightPreTag);
                Assert.AreEqual(postTag, parameters.HighlightPostTag);

                Assert.AreEqual(2, parameters.Select.Count);
                Assert.AreEqual("name", parameters.Select.First());
                Assert.AreEqual("id", parameters.Select.Last());

                Assert.AreEqual(1, parameters.OrderBy.Count);
                Assert.AreEqual("name", parameters.OrderBy.First());
            }
        }

        [TestMethod]
        public async Task TestGetSuggestions()
        {
            var suggestAsyncCalled = false;
            var suggestCalled = false;
            var searchTerm = "search";
            var preTag = "pre";
            var postTag = "post";
            var instance = new ECASuggestionParameters(searchTerm, preTag, postTag);
            Func<DocumentSuggestResponse<ECADocument>> createResponse = () =>
            {
                return new DocumentSuggestResponse<ECADocument>();
            };
            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {
                    DocumentsGet = () =>
                    {
                        return documentOperations;
                    }
                };
                var indexOperations = new StubIIndexOperations
                {
                    GetClientString = (name) =>
                    {
                        return searchIndexClient;
                    }
                };

                searchClient = new ShimSearchServiceClient();

                searchClient.IndexesGet = () =>
                {
                    return indexOperations;
                };

                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.SuggestAsyncOf1IDocumentOperationsStringStringSuggestParameters<ECADocument>((ops, srchTerm, suggesterKey, suggestParameters) =>
                {
                    Assert.AreEqual(searchTerm, srchTerm);
                    Assert.AreEqual(IndexService.DOCUMENT_NAME_SUGGESTER_KEY, suggesterKey);
                    suggestAsyncCalled = true;
                    return Task.FromResult<DocumentSuggestResponse<ECADocument>>(createResponse());
                });
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.SuggestOf1IDocumentOperationsStringStringSuggestParameters<ECADocument>((ops, srchTerm, suggesterKey, suggestParameters) =>
                {
                    Assert.AreEqual(searchTerm, srchTerm);
                    Assert.AreEqual(IndexService.DOCUMENT_NAME_SUGGESTER_KEY, suggesterKey);
                    suggestCalled = true;
                    return createResponse();
                });
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                Assert.IsFalse(suggestCalled);
                Assert.IsFalse(suggestAsyncCalled);
                service.GetSuggestions(instance, null);
                await service.GetSuggestionsAsync(instance, null);
                Assert.IsTrue(suggestCalled);
                Assert.IsTrue(suggestAsyncCalled);
            }
        }

        [TestMethod]
        public void TestGetSearchParameters()
        {
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var start = 1;
                var limit = 10;
                var fields = new List<string> { "field1" };
                var filter = "filter";
                var facets = new List<string> { "facet1" };
                var searchTerm = "search";
                var preTag = "pre";
                var postTag = "post";

                var searchParameters = new ECASearchParameters(start, limit, filter, facets, fields, searchTerm, preTag, postTag);
                var instance = service.GetSearchParameters(searchParameters, null);
                Assert.IsTrue(instance.IncludeTotalResultCount);
                Assert.AreEqual(start, instance.Skip);
                Assert.AreEqual(limit, instance.Top);
                Assert.AreEqual(filter, instance.Filter);
                Assert.AreEqual(preTag, instance.HighlightPreTag);
                Assert.AreEqual(postTag, instance.HighlightPostTag);
                CollectionAssert.AreEqual(fields, instance.Select.ToList());
                CollectionAssert.AreEqual(facets, instance.Facets.ToList());
            }
        }

        [TestMethod]
        public void TestGetSearchParameters_OnlyPreTagSupplied()
        {
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var start = 1;
                var limit = 10;
                var fields = new List<string> { "field1" };
                var filter = "filter";
                var facets = new List<string> { "facet1" };
                var searchTerm = "search";
                var preTag = "pre";
                string postTag = null;

                var searchParameters = new ECASearchParameters(start, limit, filter, facets, fields, searchTerm, preTag, postTag);
                var instance = service.GetSearchParameters(searchParameters, null);
                Assert.IsNull(instance.HighlightPreTag);
                Assert.IsNull(instance.HighlightPostTag);
            }
        }

        [TestMethod]
        public void TestGetSearchParameters_OnlyPostTagSupplied()
        {
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var start = 1;
                var limit = 10;
                var fields = new List<string> { "field1" };
                var filter = "filter";
                var facets = new List<string> { "facet1" };
                var searchTerm = "search";
                string preTag = null;
                string postTag = "post";

                var searchParameters = new ECASearchParameters(start, limit, filter, facets, fields, searchTerm, preTag, postTag);
                var instance = service.GetSearchParameters(searchParameters, null);
                Assert.IsNull(instance.HighlightPreTag);
                Assert.IsNull(instance.HighlightPostTag);
            }
        }

        [TestMethod]
        public void TestGetSearchParameters_CheckHighlightFields()
        {
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                var allFields = service.GetIndex(configuration).Fields.ToList();
                var searchableFields = allFields.Where(x => x.IsSearchable).OrderBy(x => x.Name).ToList();

                var start = 1;
                var limit = 10;
                var fields = allFields.Select(x => x.Name);
                var filter = "filter";
                var facets = new List<string> { "facet1" };
                var searchTerm = "search";
                var preTag = "pre";
                var postTag = "post";

                var searchParameters = new ECASearchParameters(start, limit, filter, facets, fields, searchTerm, preTag, postTag);
                var instance = service.GetSearchParameters(searchParameters, null);
                CollectionAssert.AreEqual(searchableFields.Select(x => x.Name).ToList(), instance.HighlightFields.OrderBy(x => x).ToList());
            }
        }

        [TestMethod]
        public void TestGetSearchParameters_DistinctFieldsAndFilters()
        {
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var start = 1;
                var limit = 10;
                var fields = new List<string> { "field1", "field1" };
                var filter = "filter";
                var facets = new List<string> { "facet1", "facet1" };
                var searchTerm = "search";
                var preTag = "pre";
                var postTag = "post";

                var searchParameters = new ECASearchParameters(start, limit, filter, facets, fields, searchTerm, preTag, postTag);
                var instance = service.GetSearchParameters(searchParameters, null);
                CollectionAssert.AreEqual(fields.Distinct().ToList(), instance.Select.ToList());
                CollectionAssert.AreEqual(facets.Distinct().ToList(), instance.Facets.ToList());
            }
        }

        [TestMethod]
        public void TestGetSearchParameters_NullFieldsAndFacets()
        {
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var start = 1;
                var limit = 10;
                List<string> fields = null;
                var filter = "filter";
                List<string> facets = null;
                var searchTerm = "search";
                var preTag = "pre";
                var postTag = "post";

                var searchParameters = new ECASearchParameters(start, limit, filter, facets, fields, searchTerm, preTag, postTag);
                var instance = service.GetSearchParameters(searchParameters, null);
                Assert.IsNotNull(instance.Select);
                Assert.IsNotNull(instance.Facets);
                Assert.AreEqual(0, instance.Select.Count);
                Assert.AreEqual(0, instance.Facets.Count);
            }
        }

        [TestMethod]
        public async Task TestSearch()
        {
            var searchCalled = false;
            var searchAsyncCalled = false;
            var start = 1;
            var limit = 10;
            var preTag = "pre";
            var postTag = "post";
            var searchParameters = new ECASearchParameters(start, limit, null, null, null, "abc", preTag, postTag);
            var documentTypeId = Guid.NewGuid();
            var key = new DocumentKey(documentTypeId, 1);
            Func<DocumentSearchResponse<ECADocument>> createDocumentSearchResponse = () =>
            {
                var response = new DocumentSearchResponse<ECADocument>();
                var document = new ECADocument();
                document.SetKey(key);
                response.Results.Add(new SearchResult<ECADocument>
                {
                    Document = document,
                });
                return response;
            };
            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {
                    DocumentsGet = () =>
                    {
                        return documentOperations;
                    }
                };
                var indexOperations = new StubIIndexOperations
                {
                    GetClientString = (name) =>
                    {
                        return searchIndexClient;
                    }
                };

                searchClient = new ShimSearchServiceClient();

                searchClient.IndexesGet = () =>
                {
                    return indexOperations;
                };

                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.SearchAsyncOf1IDocumentOperationsStringSearchParameters<ECADocument>((operations, searchTerm, parameters) =>
                {
                    Assert.AreEqual(start, parameters.Skip);
                    Assert.AreEqual(limit, parameters.Top);
                    searchAsyncCalled = true;
                    return Task.FromResult<DocumentSearchResponse<ECADocument>>(createDocumentSearchResponse());
                });
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.SearchOf1IDocumentOperationsStringSearchParameters<ECADocument>((operations, searchTerm, parameters) =>
                {
                    Assert.AreEqual(start, parameters.Skip);
                    Assert.AreEqual(limit, parameters.Top);
                    searchCalled = true;
                    return createDocumentSearchResponse();
                });
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                Action<DocumentSearchResponse<ECADocument>> tester = (response) =>
                {
                    Assert.AreEqual(1, response.Results.Count);
                    var firstResult = response.Results.First();
                    var firstDocument = firstResult.Document;
                    Assert.AreEqual(key, firstDocument.GetKey());
                };

                Assert.IsFalse(searchCalled);
                Assert.IsFalse(searchAsyncCalled);

                var docResponse = service.Search(searchParameters, null);
                var docResponseAsync = await service.SearchAsync(searchParameters, null);

                tester(docResponse);
                tester(docResponseAsync);
                Assert.IsNotNull(docResponse);
                Assert.IsNotNull(docResponseAsync);
                Assert.IsTrue(searchCalled);
                Assert.IsTrue(searchAsyncCalled);
            }
        }

        #endregion

        #region Dispose
        [TestMethod]
        public void TestDispose_SearchClient()
        {
            using (ShimsContext.Create())
            {
                var disposeCalled = false;
                searchClient = new ShimSearchServiceClient();
                Hyak.Common.Fakes.ShimServiceClient<SearchServiceClient>.AllInstances.Dispose = (x) =>
                {
                    disposeCalled = true;
                };

                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var searchClientField = typeof(IndexService).GetField("searchClient", BindingFlags.Instance | BindingFlags.NonPublic);
                var searchClientValue = searchClientField.GetValue(service);
                Assert.IsNotNull(searchClientField);
                Assert.IsNotNull(searchClientValue);

                service.Dispose();
                searchClientValue = searchClientField.GetValue(service);
                Assert.IsNull(searchClientValue);
                Assert.IsTrue(disposeCalled);
            }
        }
        #endregion

        #region Delete Documents
        [TestMethod]
        public async Task TestDeleteDocuments_DocumentKeys()
        {
            var indexCalled = false;
            var indexAsyncCalled = false;
            var documentIndexResponse = new DocumentIndexResponse();
            var configuration = new TestDocumentConfiguration(true);
            var testDocument = new TestDocument();
            var documents = new List<TestDocument>();
            documents.Add(testDocument);
            var keys = documents.Select(x => new DocumentKey(configuration.GetDocumentTypeId(), testDocument.Id)).ToList();

            Action<IndexBatch<ECADocument>, TestDocument> tester = (testBatch, expectedDocument) =>
            {
                Assert.AreEqual(1, testBatch.Actions.Count());
                var firstAction = testBatch.Actions.First();
                Assert.AreEqual(IndexActionType.Delete, firstAction.ActionType);
                Assert.AreEqual(keys.First().ToString(), firstAction.Document.Id);
            };

            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {
                    DocumentsGet = () =>
                    {
                        return documentOperations;
                    }
                };
                var indexOperations = new StubIIndexOperations
                {
                    GetClientString = (name) =>
                    {
                        return searchIndexClient;
                    }
                };

                searchClient = new ShimSearchServiceClient();

                searchClient.IndexesGet = () =>
                {
                    return indexOperations;
                };

                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexCalled = true;
                    tester(batch, testDocument);

                    return documentIndexResponse;
                });
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexAsyncOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexAsyncCalled = true;
                    tester(batch, testDocument);
                    return Task.FromResult<DocumentIndexResponse>(documentIndexResponse);
                });

                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(indexCalled);
                Assert.IsFalse(indexAsyncCalled);
                var docResponse = service.DeleteDocuments(keys);
                var docResponseAsync = await service.DeleteDocumentsAsync(keys);

                Assert.IsNotNull(docResponse);
                Assert.IsNotNull(docResponseAsync);
                Assert.IsTrue(indexCalled);
                Assert.IsTrue(indexAsyncCalled);
            }
        }

        [TestMethod]
        public async Task TestDeleteDocuments_DocumentKeysAsStrings()
        {
            var indexCalled = false;
            var indexAsyncCalled = false;
            var documentIndexResponse = new DocumentIndexResponse();
            var configuration = new TestDocumentConfiguration(true);
            var testDocument = new TestDocument();
            var documents = new List<TestDocument>();
            documents.Add(testDocument);
            var keys = documents.Select(x => new DocumentKey(configuration.GetDocumentTypeId(), testDocument.Id).ToString()).ToList();

            Action<IndexBatch<ECADocument>, TestDocument> tester = (testBatch, expectedDocument) =>
            {
                Assert.AreEqual(1, testBatch.Actions.Count());
                var firstAction = testBatch.Actions.First();
                Assert.AreEqual(IndexActionType.Delete, firstAction.ActionType);
                Assert.AreEqual(keys.First().ToString(), firstAction.Document.Id);
            };

            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {
                    DocumentsGet = () =>
                    {
                        return documentOperations;
                    }
                };
                var indexOperations = new StubIIndexOperations
                {
                    GetClientString = (name) =>
                    {
                        return searchIndexClient;
                    }
                };

                searchClient = new ShimSearchServiceClient();

                searchClient.IndexesGet = () =>
                {
                    return indexOperations;
                };

                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexCalled = true;
                    tester(batch, testDocument);

                    return documentIndexResponse;
                });
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexAsyncOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexAsyncCalled = true;
                    tester(batch, testDocument);
                    return Task.FromResult<DocumentIndexResponse>(documentIndexResponse);
                });

                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(indexCalled);
                Assert.IsFalse(indexAsyncCalled);
                var docResponse = service.DeleteDocuments(keys);
                var docResponseAsync = await service.DeleteDocumentsAsync(keys);

                Assert.IsNotNull(docResponse);
                Assert.IsNotNull(docResponseAsync);
                Assert.IsTrue(indexCalled);
                Assert.IsTrue(indexAsyncCalled);
            }
        }

        [TestMethod]
        public async Task TestDeleteDocuments_CheckDistinctDocumentKeysAsStrings()
        {
            var indexCalled = false;
            var indexAsyncCalled = false;
            var documentIndexResponse = new DocumentIndexResponse();
            var configuration = new TestDocumentConfiguration(true);
            var testDocument = new TestDocument();
            var documents = new List<TestDocument>();
            documents.Add(testDocument);
            var keys = documents.Select(x => new DocumentKey(configuration.GetDocumentTypeId(), testDocument.Id).ToString()).ToList();
            keys.Add(keys[0]);

            Action<IndexBatch<ECADocument>, TestDocument> tester = (testBatch, expectedDocument) =>
            {
                Assert.AreEqual(2, keys.Count);
                Assert.AreEqual(1, testBatch.Actions.Count());
            };

            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {
                    DocumentsGet = () =>
                    {
                        return documentOperations;
                    }
                };
                var indexOperations = new StubIIndexOperations
                {
                    GetClientString = (name) =>
                    {
                        return searchIndexClient;
                    }
                };

                searchClient = new ShimSearchServiceClient();

                searchClient.IndexesGet = () =>
                {
                    return indexOperations;
                };

                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexCalled = true;
                    tester(batch, testDocument);

                    return documentIndexResponse;
                });
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexAsyncOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexAsyncCalled = true;
                    tester(batch, testDocument);
                    return Task.FromResult<DocumentIndexResponse>(documentIndexResponse);
                });

                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(indexCalled);
                Assert.IsFalse(indexAsyncCalled);
                var docResponse = service.DeleteDocuments(keys);
                var docResponseAsync = await service.DeleteDocumentsAsync(keys);

                Assert.IsNotNull(docResponse);
                Assert.IsNotNull(docResponseAsync);
                Assert.IsTrue(indexCalled);
                Assert.IsTrue(indexAsyncCalled);
            }
        }

        [TestMethod]
        public async Task TestDeleteDocuments_CheckDistinctDocumentKeys()
        {
            var indexCalled = false;
            var indexAsyncCalled = false;
            var documentIndexResponse = new DocumentIndexResponse();
            var configuration = new TestDocumentConfiguration(true);
            var testDocument = new TestDocument();
            var documents = new List<TestDocument>();
            documents.Add(testDocument);
            var keys = documents.Select(x => new DocumentKey(configuration.GetDocumentTypeId(), testDocument.Id)).ToList();
            keys.Add(keys[0]);

            Action<IndexBatch<ECADocument>, TestDocument> tester = (testBatch, expectedDocument) =>
            {
                Assert.AreEqual(2, keys.Count);
                Assert.AreEqual(1, testBatch.Actions.Count());
            };

            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {
                    DocumentsGet = () =>
                    {
                        return documentOperations;
                    }
                };
                var indexOperations = new StubIIndexOperations
                {
                    GetClientString = (name) =>
                    {
                        return searchIndexClient;
                    }
                };

                searchClient = new ShimSearchServiceClient();

                searchClient.IndexesGet = () =>
                {
                    return indexOperations;
                };

                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexCalled = true;
                    tester(batch, testDocument);

                    return documentIndexResponse;
                });
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexAsyncOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexAsyncCalled = true;
                    tester(batch, testDocument);
                    return Task.FromResult<DocumentIndexResponse>(documentIndexResponse);
                });

                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(indexCalled);
                Assert.IsFalse(indexAsyncCalled);
                var docResponse = service.DeleteDocuments(keys);
                var docResponseAsync = await service.DeleteDocumentsAsync(keys);

                Assert.IsNotNull(docResponse);
                Assert.IsNotNull(docResponseAsync);
                Assert.IsTrue(indexCalled);
                Assert.IsTrue(indexAsyncCalled);
            }
        }

        [TestMethod]
        public async Task TestDeleteDocuments_NoDocumentKeys()
        {
            var indexCalled = false;
            var indexAsyncCalled = false;
            var documentIndexResponse = new DocumentIndexResponse();
            var configuration = new TestDocumentConfiguration(true);


            using (ShimsContext.Create())
            {
                var documentOperations = new StubIDocumentOperations
                {
                };
                var searchIndexClient = new ShimSearchIndexClient
                {
                    DocumentsGet = () =>
                    {
                        return documentOperations;
                    }
                };
                var indexOperations = new StubIIndexOperations
                {
                    GetClientString = (name) =>
                    {
                        return searchIndexClient;
        }
                };

                searchClient = new ShimSearchServiceClient();

                searchClient.IndexesGet = () =>
                {
                    return indexOperations;
                };

                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexCalled = true;
                    return documentIndexResponse;
                });
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexAsyncOf1IDocumentOperationsIndexBatchOfM0<ECADocument>((operations, batch) =>
                {
                    indexAsyncCalled = true;
                    return Task.FromResult<DocumentIndexResponse>(documentIndexResponse);
                });

                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(indexCalled);
                Assert.IsFalse(indexAsyncCalled);
                var docResponse = service.DeleteDocuments(new List<string>());
                var docResponseAsync = await service.DeleteDocumentsAsync(new List<string>());

                Assert.IsNull(docResponse);
                Assert.IsNull(docResponseAsync);
                Assert.IsFalse(indexCalled);
                Assert.IsFalse(indexAsyncCalled);
            }
        }
        #endregion


        [TestMethod]
        public void TestGetAllConfigurations()
        {
            var t = this.GetType();
            var allConfigurations = IndexService.GetAllConfigurations(t.Assembly);
            Assert.AreEqual(3, allConfigurations.Count());
            var list = allConfigurations.ToList();
            foreach (var config in list)
            {
                var configType = config.GetType();
                Assert.IsFalse(configType.IsAbstract);
                Assert.IsTrue(configType.IsClass);
                Assert.IsFalse(configType.IsInterface);
                Assert.IsTrue(typeof(IDocumentConfiguration).IsAssignableFrom(configType));
            }
        }

        [TestMethod]
        public void TestGetDocumentFieldNames()
        {
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var fields = service.GetDocumentFieldNames();
                var index = service.GetIndex(configuration);
                CollectionAssert.AreEqual(index.Fields.Select(x => x.Name).OrderBy(x => x).ToList(), fields.OrderBy(x => x).ToList());
            }
        }

        [TestMethod]
        public void TestGetIndex()
        {
            var testDocument = new TestDocument();
            testDocument.Id = 1;
            testDocument.Description = "desc";
            testDocument.Name = "name";

            Func<string, string> toCamelCase = (s) =>
            {
                return s.Substring(0, 1).ToLower() + s.Substring(1);
            };

            var testDocumentConfiguration = new TestDocumentConfiguration();
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var configuration = new TestDocumentConfiguration(true);
                service = new IndexService(this.indexName, searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var index = service.GetIndex(testDocumentConfiguration);
                Assert.AreEqual(this.indexName, index.Name);

                Assert.AreEqual(1, index.Suggesters.Count);
                var firstSuggester = index.Suggesters.First();
                Assert.AreEqual(IndexService.DOCUMENT_NAME_SUGGESTER_KEY, firstSuggester.Name);
                Assert.AreEqual(SuggesterSearchMode.AnalyzingInfixMatching, firstSuggester.SearchMode);
                Assert.AreEqual(1, firstSuggester.SourceFields.Count);
                Assert.AreEqual(toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(x => x.Name)), firstSuggester.SourceFields.First());
                var allFieldNames = typeof(ECADocument).GetProperties().Select(x => toCamelCase(x.Name)).ToList();

                var idField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.Id))).FirstOrDefault();
                Assert.IsNotNull(idField);
                Assert.IsTrue(idField.IsKey);
                Assert.IsTrue(idField.IsRetrievable);
                Assert.IsFalse(idField.IsSearchable);
                allFieldNames.Remove(idField.Name);

                var nameField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.Name))).FirstOrDefault();
                Assert.IsNotNull(nameField);
                Assert.IsTrue(nameField.IsRetrievable);
                Assert.IsTrue(nameField.IsSearchable);
                Assert.IsTrue(nameField.IsSortable);
                Assert.AreEqual(DataType.String, nameField.Type);
                allFieldNames.Remove(nameField.Name);

                var descriptionField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.Description))).FirstOrDefault();
                Assert.IsNotNull(descriptionField);
                Assert.IsTrue(descriptionField.IsRetrievable);
                Assert.IsTrue(descriptionField.IsSearchable);
                Assert.AreEqual(DataType.String, descriptionField.Type);
                allFieldNames.Remove(descriptionField.Name);

                var fociField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.Foci))).FirstOrDefault();
                Assert.IsNotNull(fociField);
                Assert.IsTrue(fociField.IsRetrievable);
                Assert.IsTrue(fociField.IsSearchable);
                Assert.AreEqual(DataType.Collection(DataType.String), fociField.Type);
                allFieldNames.Remove(fociField.Name);

                var objectivesField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.Objectives))).FirstOrDefault();
                Assert.IsNotNull(objectivesField);
                Assert.IsTrue(objectivesField.IsRetrievable);
                Assert.IsTrue(objectivesField.IsSearchable);
                Assert.AreEqual(DataType.Collection(DataType.String), objectivesField.Type);
                allFieldNames.Remove(objectivesField.Name);

                var themesField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.Themes))).FirstOrDefault();
                Assert.IsNotNull(themesField);
                Assert.IsTrue(themesField.IsRetrievable);
                Assert.IsTrue(themesField.IsSearchable);
                Assert.AreEqual(DataType.Collection(DataType.String), themesField.Type);
                allFieldNames.Remove(themesField.Name);

                var goalsField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.Goals))).FirstOrDefault();
                Assert.IsNotNull(goalsField);
                Assert.IsTrue(goalsField.IsRetrievable);
                Assert.IsTrue(goalsField.IsSearchable);
                Assert.AreEqual(DataType.Collection(DataType.String), goalsField.Type);
                allFieldNames.Remove(goalsField.Name);

                var pocField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.PointsOfContact))).FirstOrDefault();
                Assert.IsNotNull(pocField);
                Assert.IsTrue(pocField.IsRetrievable);
                Assert.IsTrue(pocField.IsSearchable);
                Assert.AreEqual(DataType.Collection(DataType.String), pocField.Type);
                allFieldNames.Remove(pocField.Name);

                var regionsField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.Regions))).FirstOrDefault();
                Assert.IsNotNull(regionsField);
                Assert.IsTrue(regionsField.IsRetrievable);
                Assert.IsTrue(regionsField.IsSearchable);
                Assert.AreEqual(DataType.Collection(DataType.String), regionsField.Type);
                allFieldNames.Remove(regionsField.Name);

                var countriesField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.Countries))).FirstOrDefault();
                Assert.IsNotNull(countriesField);
                Assert.IsTrue(countriesField.IsRetrievable);
                Assert.IsTrue(countriesField.IsSearchable);
                Assert.AreEqual(DataType.Collection(DataType.String), countriesField.Type);
                allFieldNames.Remove(countriesField.Name);

                var locationsField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.Locations))).FirstOrDefault();
                Assert.IsNotNull(locationsField);
                Assert.IsTrue(locationsField.IsRetrievable);
                Assert.IsTrue(locationsField.IsSearchable);
                Assert.AreEqual(DataType.Collection(DataType.String), locationsField.Type);
                allFieldNames.Remove(locationsField.Name);

                var websitesField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.Websites))).FirstOrDefault();
                Assert.IsNotNull(websitesField);
                Assert.IsTrue(websitesField.IsRetrievable);
                Assert.IsTrue(websitesField.IsSearchable);
                Assert.AreEqual(DataType.Collection(DataType.String), websitesField.Type);
                allFieldNames.Remove(websitesField.Name);

                var docTypeNameField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.DocumentTypeName))).FirstOrDefault();
                Assert.IsNotNull(docTypeNameField);
                Assert.IsTrue(docTypeNameField.IsRetrievable);
                Assert.IsTrue(docTypeNameField.IsSearchable);
                Assert.AreEqual(DataType.String, docTypeNameField.Type);
                allFieldNames.Remove(docTypeNameField.Name);

                var statusField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.Status))).FirstOrDefault();
                Assert.IsNotNull(statusField);
                Assert.IsTrue(statusField.IsRetrievable);
                Assert.IsTrue(statusField.IsSearchable);
                Assert.AreEqual(DataType.String, statusField.Type);
                allFieldNames.Remove(statusField.Name);

                var docTypeIdField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.DocumentTypeId))).FirstOrDefault();
                Assert.IsNotNull(docTypeIdField);
                Assert.IsTrue(docTypeIdField.IsRetrievable);
                Assert.IsFalse(docTypeIdField.IsSearchable);
                Assert.AreEqual(DataType.String, docTypeIdField.Type);
                allFieldNames.Remove(docTypeIdField.Name);

                var officeSymbolField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.OfficeSymbol))).FirstOrDefault();
                Assert.IsNotNull(officeSymbolField);
                Assert.IsTrue(officeSymbolField.IsRetrievable);
                Assert.IsTrue(officeSymbolField.IsSearchable);
                Assert.AreEqual(DataType.String, officeSymbolField.Type);
                allFieldNames.Remove(officeSymbolField.Name);

                var startDateField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.StartDate))).FirstOrDefault();
                Assert.IsNotNull(startDateField);
                Assert.IsTrue(startDateField.IsRetrievable);
                Assert.IsTrue(startDateField.IsFilterable);
                Assert.AreEqual(DataType.DateTimeOffset, startDateField.Type);
                allFieldNames.Remove(startDateField.Name);

                var endDateField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.EndDate))).FirstOrDefault();
                Assert.IsNotNull(endDateField);
                Assert.IsTrue(endDateField.IsRetrievable);
                Assert.IsTrue(endDateField.IsFilterable);
                Assert.AreEqual(DataType.DateTimeOffset, endDateField.Type);
                allFieldNames.Remove(endDateField.Name);

                var addressesField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.Addresses))).FirstOrDefault();
                Assert.IsNotNull(addressesField);
                Assert.IsTrue(addressesField.IsRetrievable);
                Assert.IsFalse(addressesField.IsFilterable);
                Assert.AreEqual(DataType.Collection(DataType.String), addressesField.Type);
                allFieldNames.Remove(addressesField.Name);

                var phoneNumbersField = index.Fields.Where(x => x.Name == toCamelCase(PropertyHelper.GetPropertyName<ECADocument>(y => y.PhoneNumbers))).FirstOrDefault();
                Assert.IsNotNull(phoneNumbersField);
                Assert.IsTrue(phoneNumbersField.IsRetrievable);
                Assert.IsFalse(phoneNumbersField.IsFilterable);
                Assert.AreEqual(DataType.Collection(DataType.String), phoneNumbersField.Type);
                allFieldNames.Remove(phoneNumbersField.Name);

                Assert.AreEqual(0, allFieldNames.Count, "Make sure all fields are checked from ECADocument and the index.  Some have not been checked.");
            }
        }
    }
}
