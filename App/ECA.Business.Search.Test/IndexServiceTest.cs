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

namespace ECA.Business.Search.Test
{

    [TestClass]
    public class IndexServiceTest
    {
        private string searchServiceName = "ecakmtsrch-dev";
        private string apikey = "494093D1A885C79D809ACAD6EB20F5AC";

        private ShimSearchServiceClient searchClient;
        private IndexService service;

        [TestInitialize]
        public void TestInit()
        {

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
                    return Task.FromResult<bool>(doesExist);
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsIIndexOperationsString = (operations, indexName) =>
                {
                    return doesExist;
                };

                service = new IndexService(searchClient.Instance);
                Assert.AreEqual(doesExist, service.Exists(DocumentType.Program));
                Assert.AreEqual(doesExist, await service.ExistsAsync(DocumentType.Program));
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
                    return Task.FromResult<bool>(doesExist);
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsIIndexOperationsString = (operations, indexName) =>
                {
                    return doesExist;
                };

                service = new IndexService(searchClient.Instance);
                Assert.AreEqual(doesExist, service.Exists(DocumentType.Program));
                Assert.AreEqual(doesExist, await service.ExistsAsync(DocumentType.Program));
            }
        }

        #endregion

        #region GetDocumentById
        [TestMethod]
        public async Task TestGetDocumentById_String()
        {
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
                    return Task.FromResult<DocumentGetResponse<ECADocument>>(documentGetResponse);
                });
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.GetOf1IDocumentOperationsString<ECADocument>((operations, id) =>
                {
                    getCalled = true;
                    return documentGetResponse;
                });


                var configuration = new TestDocumentConfiguration();
                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(getAsyncCalled);
                Assert.IsFalse(getCalled);
                var key = new DocumentKey(DocumentType.Program, 1);
                service.GetDocumentById(key.ToString());
                await service.GetDocumentByIdAsync(key.ToString());
                Assert.IsTrue(getAsyncCalled);
                Assert.IsTrue(getCalled);
            }
        }


        [TestMethod]
        public async Task TestGetDocumentById_DocumentKey()
        {
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
                    return Task.FromResult<DocumentGetResponse<ECADocument>>(documentGetResponse);
                });
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.GetOf1IDocumentOperationsString<ECADocument>((operations, id) =>
                {
                    getCalled = true;
                    return documentGetResponse;
                });


                var configuration = new TestDocumentConfiguration();
                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(getAsyncCalled);
                Assert.IsFalse(getCalled);
                var key = new DocumentKey(DocumentType.Program, 1);
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
                    //ExistsAsyncStringCancellationToken = (indexName, cancelToken) =>
                    //{
                    //    return Task.FromResult<bool>(doesExist);
                    //},
                };
                searchClient.IndexesGet = () =>
                {
                    return index;
                };

                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.CreateOrUpdateAsyncIIndexOperationsIndex = (operations, indexName) =>
                {
                    calledCreateAsync = true;
                    return Task.FromResult<IndexDefinitionResponse>(indexDefinitionResponse);
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.CreateOrUpdateIIndexOperationsIndex = (operations, indexName) =>
                {
                    calledCreate = true;
                    return indexDefinitionResponse;
                };

                var configuration = new TestDocumentConfiguration();
                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { configuration });
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
                    //ExistsAsyncStringCancellationToken = (indexName, cancelToken) =>
                    //{
                    //    return Task.FromResult<bool>(doesExist);
                    //},
                };
                searchClient.IndexesGet = () =>
                {
                    return index;
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsAsyncIIndexOperationsString = (operations, indexName) =>
                {
                    calledDoesExistAsync = true;
                    return Task.FromResult<bool>(doesExist);
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsIIndexOperationsString = (operations, indexName) =>
                {
                    calledDoesExist = true;
                    return doesExist;
                };

                //Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.DeleteAsyncIIndexOperationsString = (operations, indexName) =>
                //{
                //    calledDeleteAsync = true;
                //    return Task.FromResult<AzureOperationResponse>(response);
                //};
                //Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.DeleteIIndexOperationsString = (operations, indexName) =>
                //{
                //    calledDelete = true;
                //    return response;
                //};

                var configuration = new TestDocumentConfiguration();
                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(calledDelete);
                Assert.IsFalse(calledDeleteAsync);
                Assert.IsFalse(calledDoesExist);
                Assert.IsFalse(calledDoesExistAsync);
                var testDocument = new TestDocument();
                service.DeleteIndex(configuration.GetDocumentType());
                await service.DeleteIndexAsync(configuration.GetDocumentType());
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
                    //ExistsAsyncStringCancellationToken = (indexName, cancelToken) =>
                    //{
                    //    return Task.FromResult<bool>(doesExist);
                    //},
                };
                searchClient.IndexesGet = () =>
                {
                    return index;
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsAsyncIIndexOperationsString = (operations, indexName) =>
                {
                    calledDoesExistAsync = true;
                    return Task.FromResult<bool>(doesExist);
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.ExistsIIndexOperationsString = (operations, indexName) =>
                {
                    calledDoesExist = true;
                    return doesExist;
                };

                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.DeleteAsyncIIndexOperationsString = (operations, indexName) =>
                {
                    calledDeleteAsync = true;
                    return Task.FromResult<AzureOperationResponse>(response);
                };
                Microsoft.Azure.Search.Fakes.ShimIndexOperationsExtensions.DeleteIIndexOperationsString = (operations, indexName) =>
                {
                    calledDelete = true;
                    return response;
                };
                var configuration = new TestDocumentConfiguration();
                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { configuration });
                Assert.IsFalse(calledDelete);
                Assert.IsFalse(calledDeleteAsync);
                Assert.IsFalse(calledDoesExist);
                Assert.IsFalse(calledDoesExistAsync);
                var testDocument = new TestDocument();
                service.DeleteIndex(configuration.GetDocumentType());
                await service.DeleteIndexAsync(configuration.GetDocumentType());
                Assert.IsTrue(calledDelete);
                Assert.IsTrue(calledDeleteAsync);
                Assert.IsTrue(calledDoesExist);
                Assert.IsTrue(calledDoesExistAsync);
            }
        }
        #endregion

        #region Handle Documents
        [TestMethod]
        public async Task TestHandleDocuments()
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
                var configuration = new TestDocumentConfiguration();
                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var testDocument = new TestDocument();
                var documents = new List<TestDocument>();
                documents.Add(new TestDocument());
                Assert.IsFalse(indexCalled);
                Assert.IsFalse(indexAsyncCalled);
                var docResponse = service.HandleDocuments(documents);
                var docResponseAsync = await service.HandleDocumentsAsync(documents);

                Assert.IsNotNull(docResponse);
                Assert.IsNotNull(docResponseAsync);
                Assert.IsTrue(indexCalled);
                Assert.IsTrue(indexAsyncCalled);
            }
        }

        [TestMethod]
        public async Task TestHandleDocuments_NoDocumentsProvided()
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

                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexIDocumentOperationsIndexBatch = (operations, batch) =>
                {
                    indexCalled = true;
                    return documentIndexResponse;
                };
                Microsoft.Azure.Search.Fakes.ShimDocumentOperationsExtensions.IndexAsyncIDocumentOperationsIndexBatch = (operations, batch) =>
                {
                    indexAsyncCalled = true;
                    return Task.FromResult<DocumentIndexResponse>(documentIndexResponse);
                };
                var configuration = new TestDocumentConfiguration();
                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var testDocument = new TestDocument();
                var documents = new List<TestDocument>();
                Assert.IsFalse(indexCalled);
                Assert.IsFalse(indexAsyncCalled);
                var docResponse = service.HandleDocuments(documents);
                var docResponseAsync = await service.HandleDocumentsAsync(documents);

                Assert.IsNull(docResponse);
                Assert.IsNull(docResponseAsync);
                Assert.IsFalse(indexCalled);
                Assert.IsFalse(indexAsyncCalled);
            }
        }

        [TestMethod]
        public async Task TestHandleDocuments_ConfigurationNotProvided()
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


                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration>());

                var testDocument = new TestDocument();
                var documents = new List<TestDocument>();
                documents.Add(new TestDocument());

                var message = String.Format("The configuration for the type [{0}] was not found.", typeof(TestDocument));
                Func<Task> f = () =>
                {
                    return service.HandleDocumentsAsync(documents);
                };

                Action a = () => service.HandleDocuments(documents);
                a.ShouldThrow<NotSupportedException>().WithMessage(message);
                f.ShouldThrow<NotSupportedException>().WithMessage(message);
            }
        }

        #endregion

        #region Constructor
        [TestMethod]
        public void TestConstructor_DistinctConfigurations()
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
                var instance2 = new TestDocumentConfiguration();
                Action a = () => new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { instance1, instance2 });
                a.ShouldThrow<NotSupportedException>().WithMessage("The following document types are configured more than once:  Program.");
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
                instance1.IsDocumentType(null);
                Action a = () => new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { instance1 });
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
                var instance1 = new TestDocumentConfiguration();
                var instance2 = new OtherTestDocumentConfiguration();
                var service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { instance1, instance2 });
                Assert.AreEqual(2, service.Configurations.Count);
            }
        }
        #endregion

        #region Search
        [TestMethod]
        public void TestGetSearchParameters()
        {
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();                
                var configuration = new TestDocumentConfiguration();
                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var start = 1;
                var limit = 10;
                var fields = new List<string> { "field1" };
                var filter = "filter";
                var facets = new List<string> { "facet1" };
                var searchTerm = "search";

                var searchParameters = new ECASearchParameters(start, limit, filter, facets, fields, searchTerm);
                var instance = service.GetSearchParameters(searchParameters, null);
                Assert.AreEqual(start, instance.Skip);
                Assert.AreEqual(limit, instance.Top);
                CollectionAssert.AreEqual(fields, instance.Select.ToList());
                CollectionAssert.AreEqual(facets, instance.Facets.ToList());
            }
        }

        [TestMethod]
        public void TestGetSearchParameters_DistinctFieldsAndFilters()
        {
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var configuration = new TestDocumentConfiguration();
                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var start = 1;
                var limit = 10;
                var fields = new List<string> { "field1", "field1" };
                var filter = "filter";
                var facets = new List<string> { "facet1", "facet1" };
                var searchTerm = "search";

                var searchParameters = new ECASearchParameters(start, limit, filter, facets, fields, searchTerm);
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
                var configuration = new TestDocumentConfiguration();
                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var start = 1;
                var limit = 10;
                List<string> fields = null;
                var filter = "filter";
                List<string> facets = null;
                var searchTerm = "search";

                var searchParameters = new ECASearchParameters(start, limit, filter, facets, fields, searchTerm);
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

            var key = new DocumentKey(DocumentType.Program, 1);
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
                var configuration = new TestDocumentConfiguration();
                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                Action<DocumentSearchResponse<ECADocument>> tester = (response) =>
                {
                    Assert.AreEqual(1, response.Results.Count);
                    var firstResult = response.Results.First();
                    var firstDocument = firstResult.Document;
                    Assert.AreEqual(key, firstDocument.GetKey());
                };

                Assert.IsFalse(searchCalled);
                Assert.IsFalse(searchAsyncCalled);
                var searchParameters = new ECASearchParameters(start, limit, null, null, null, "abc");
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

                var configuration = new TestDocumentConfiguration();
                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { configuration });

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

        [TestMethod]
        public void TestGetIndex()
        {
            var testDocument = new TestDocument();
            testDocument.Id = 1;
            testDocument.Description = "desc";
            testDocument.Name = "name";

            var testDocumentConfiguration = new TestDocumentConfiguration();
            using (ShimsContext.Create())
            {
                searchClient = new ShimSearchServiceClient();
                var configuration = new TestDocumentConfiguration();
                service = new IndexService(searchClient.Instance, new List<IDocumentConfiguration> { configuration });

                var index = service.GetIndex(testDocumentConfiguration);
                Assert.AreEqual(configuration.GetDocumentType().IndexName, index.Name);

                var idField = index.Fields.Where(x => x.Name == PropertyHelper.GetPropertyName<ECADocument>(y => y.Id)).FirstOrDefault();
                Assert.IsNotNull(idField);
                Assert.IsTrue(idField.IsKey);
                Assert.IsTrue(idField.IsRetrievable);
                Assert.IsFalse(idField.IsSearchable);

                var titleField = index.Fields.Where(x => x.Name == PropertyHelper.GetPropertyName<ECADocument>(y => y.Name)).FirstOrDefault();
                Assert.IsNotNull(titleField);
                Assert.IsTrue(titleField.IsRetrievable);
                Assert.IsTrue(titleField.IsSearchable);

                var descriptionField = index.Fields.Where(x => x.Name == PropertyHelper.GetPropertyName<ECADocument>(y => y.Description)).FirstOrDefault();
                Assert.IsNotNull(descriptionField);
                Assert.IsTrue(descriptionField.IsRetrievable);
                Assert.IsTrue(descriptionField.IsSearchable);

                var fociField = index.Fields.Where(x => x.Name == PropertyHelper.GetPropertyName<ECADocument>(y => y.Foci)).FirstOrDefault();
                Assert.IsNotNull(fociField);
                Assert.IsTrue(fociField.IsRetrievable);
                Assert.IsTrue(fociField.IsSearchable);

                var objectivesField = index.Fields.Where(x => x.Name == PropertyHelper.GetPropertyName<ECADocument>(y => y.Objectives)).FirstOrDefault();
                Assert.IsNotNull(objectivesField);
                Assert.IsTrue(objectivesField.IsRetrievable);
                Assert.IsTrue(objectivesField.IsSearchable);

                var themesField = index.Fields.Where(x => x.Name == PropertyHelper.GetPropertyName<ECADocument>(y => y.Themes)).FirstOrDefault();
                Assert.IsNotNull(themesField);
                Assert.IsTrue(themesField.IsRetrievable);
                Assert.IsTrue(themesField.IsSearchable);

                var goalsField = index.Fields.Where(x => x.Name == PropertyHelper.GetPropertyName<ECADocument>(y => y.Goals)).FirstOrDefault();
                Assert.IsNotNull(goalsField);
                Assert.IsTrue(goalsField.IsRetrievable);
                Assert.IsTrue(goalsField.IsSearchable);

                var pocField = index.Fields.Where(x => x.Name == PropertyHelper.GetPropertyName<ECADocument>(y => y.PointsOfContact)).FirstOrDefault();
                Assert.IsNotNull(pocField);
                Assert.IsTrue(pocField.IsRetrievable);
                Assert.IsTrue(pocField.IsSearchable);

                var docTypeField = index.Fields.Where(x => x.Name == PropertyHelper.GetPropertyName<ECADocument>(y => y.DocumentType)).FirstOrDefault();
                Assert.IsNotNull(docTypeField);
                Assert.IsTrue(docTypeField.IsRetrievable);
                Assert.IsTrue(docTypeField.IsSearchable);

                var officeSymbolField = index.Fields.Where(x => x.Name == PropertyHelper.GetPropertyName<ECADocument>(y => y.OfficeSymbol)).FirstOrDefault();
                Assert.IsNotNull(officeSymbolField);
                Assert.IsTrue(officeSymbolField.IsRetrievable);
                Assert.IsTrue(officeSymbolField.IsSearchable);
            }
        }

        //[TestMethod]
        //public async Task Test()
        //{
        //    var instance = new TestDocument
        //    {
        //        Description = "description",
        //        Name = "name",
        //        Id = 1,
        //        AdditionalField = "additional field",
        //        Subtitle = "subtitle"
        //    };


        //    service = new IndexService(
        //        new SearchServiceClient(searchServiceName, new SearchCredentials(apikey)),
        //        new List<IDocumentConfiguration>
        //        {
        //            new TestDocumentConfiguration()
        //        }
        //        );
        //    if (await service.ExistsAsync(DocumentType.Program))
        //    {
        //        await service.DeleteIndexAsync(DocumentType.Program);
        //    }
        //    await service.CreateIndexAsync<TestDocument>();

        //    //await service.DeleteIndexAsync(DocumentType.Program);
        //    //await service.CreateIndexAsync<TestDocument>();


        //    var documents = new List<TestDocument>();
        //    documents.Add(instance);
        //    await service.CreateIndexAsync<TestDocument>();
        //    var response = await service.HandleDocumentsAsync(documents);

        //    var search = await service.SearchAsync(instance.Name, null);
        //    Assert.AreEqual(1, search.Results.Count);

        //}
    }
}
