using ECA.Core.Exceptions;
using ECA.Core.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    #region IDocumentService
    /// <summary>
    /// An IDocumentService is capable of retrieving and indexing documents from a datastore.
    /// </summary>
    [ContractClass(typeof(BasicDocumentServiceContract))]
    public interface IDocumentService
    {
        /// <summary>
        /// Gets the number of documents to process.
        /// </summary>
        /// <returns>The number of documents to process.</returns>
        int GetDocumentCount();

        /// <summary>
        /// Gets the number of documents to process.
        /// </summary>
        /// <returns>The number of documents to process.</returns>
        Task<int> GetDocumentCountAsync();

        /// <summary>
        /// Add or update all documents.
        /// </summary>
        void AddOrUpdateAll();

        /// <summary>
        /// Processes the documents.
        /// </summary>
        Task AddOrUpdateAllAsync();

        /// <summary>
        /// Performs an add or update on a single document.
        /// </summary>
        /// <param name="id">The id of the entity whose document should be updated or added.</param>
        void AddOrUpdateDocument(object id);

        /// <summary>
        /// Performs an add or update on a single document.
        /// </summary>
        /// <param name="id">The id of the entity whose document should be updated or added.</param>
        Task AddOrUpdateDocumentAsync(object id);

        /// <summary>
        /// Deletes the documents whose entity ids are equal to the given ids.
        /// </summary>
        /// <param name="ids">The ids of the entities whose documents should be deleted.</param>
        void DeleteDocuments(List<object> ids);

        /// <summary>
        /// Deletes the documents whose entity ids are equal to the given ids.
        /// </summary>
        /// <param name="ids">The ids of the entities whose documents should be deleted.</param>
        Task DeleteDocumentsAsync(List<object> ids);

        /// <summary>
        /// Returns the document type id for the documents this service will handle.
        /// </summary>
        /// <returns>The document type id for the documents this service will handle.</returns>
        Guid GetDocumentTypeId();
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IDocumentService))]
    public abstract class BasicDocumentServiceContract : IDocumentService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetDocumentCount()
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<int> GetDocumentCountAsync()
        {
            return Task.FromResult<int>(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Guid GetDocumentTypeId()
        {
            Contract.Ensures(Contract.Result<Guid>() != Guid.Empty, "The document type id must not be the empty guid.");
            return Guid.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddOrUpdateAll()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task AddOrUpdateAllAsync()
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void AddOrUpdateDocument(object id)
        {
            Contract.Requires(id != null, "The id must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task AddOrUpdateDocumentAsync(object id)
        {
            Contract.Requires(id != null, "The id must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteDocuments(List<object> ids)
        {
            Contract.Requires(ids != null, "The ids must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task DeleteDocumentsAsync(List<object> ids)
        {
            Contract.Requires(ids != null, "The ids must not be null.");
            return Task.FromResult<object>(null);
        }
    }
    #endregion

    #region Typed IDocumentService<TDocument>
    /// <summary>
    /// A IDocumentService that is capable of processing documents of T in batches.
    /// </summary>
    /// <typeparam name="TDocument">The type of document to process.</typeparam>
    
    public interface IDocumentService<TDocument> : IDocumentService where TDocument : class
    {
        /// <summary>
        /// Returns a batch of documents to process.
        /// </summary>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="take">The number of records in the batch.</param>
        /// <returns>The batch of documents.</returns>
        List<TDocument> GetDocumentBatch(int skip, int take);

        /// <summary>
        /// Returns a batch of documents to process.
        /// </summary>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="take">The number of records in the batch.</param>
        /// <returns>The batch of documents.</returns>
        Task<List<TDocument>> GetDocumentBatchAsync(int skip, int take);
    }
    #endregion

    #region DbContext DocumentService
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TDocument"></typeparam>
    [ContractClassFor(typeof(DocumentService<,>))]
    public abstract class DocumentServiceContract<TContext, TDocument> : DocumentService<TContext, TDocument>
        where TContext : DbContext
        where TDocument : class
    {
        public DocumentServiceContract(TContext context, IIndexService indexService, IIndexNotificationService notificationService)
            : base(context, indexService, notificationService)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(indexService != null, "The index service must not be null.");
            Contract.Requires(notificationService != null, "The notification service must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override IQueryable<TDocument> CreateGetDocumentByIdQuery(object id)
        {
            Contract.Requires(id != null, "The id must not be null.");
            Contract.Ensures(Contract.Result<IQueryable<TDocument>>() != null, "The query must not return null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IQueryable<TDocument> CreateGetDocumentsQuery()
        {
            Contract.Ensures(Contract.Result<IQueryable<TDocument>>() != null, "The query must not return null.");
            return null;
        }        
    }

    /// <summary>
    /// A DocumentService that is capable of processing documents in a DbContext in batches using configurations.
    /// </summary>
    /// <typeparam name="TContext">The context datastore type containg documents of T.</typeparam>
    /// <typeparam name="TDocument">The object type that will be translated to documents.</typeparam>
    [ContractClass(typeof(DocumentServiceContract<,>))]
    public abstract class DocumentService<TContext, TDocument> : DbContextService<TContext>, IDocumentService<TDocument>
        where TContext : DbContext
        where TDocument : class
    {
        /// <summary>
        /// The default batch size of documents to process.
        /// </summary>
        public const int DEFAULT_BATCH_SIZE = 100;
        
        private readonly int batchSize;
        private IIndexService indexService;
        private IIndexNotificationService notificationService;
        private Action<IDocumentConfiguration, Type> throwIfDocumentConfigurationNotFound;
        private Action<TDocument, string, object> throwIfDocumentNotFound;

        /// <summary>
        /// Creates a new DocumentService with the given context to query, the index service to index documents with and
        /// the notification service to supply indexing updates to.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="indexService">The index service.</param>
        /// <param name="notificationService">The notification service.</param>
        /// <param name="batchSize">The batch size of documents to process at one time.</param>
        public DocumentService(TContext context, IIndexService indexService, IIndexNotificationService notificationService, int batchSize = DEFAULT_BATCH_SIZE) : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(indexService != null, "The index service must not be null.");
            Contract.Requires(notificationService != null, "The notification service must not be null.");
            this.indexService = indexService;
            this.batchSize = batchSize;
            this.notificationService = notificationService;
            context.Configuration.AutoDetectChangesEnabled = false;
            throwIfDocumentConfigurationNotFound = (config, t) =>
            {
                if (config == null)
                {
                    throw new NotSupportedException(String.Format("The document configuration for the type [{0}] was not found.", t));
                }
            };
            throwIfDocumentNotFound = (doc, documentTypeName, id) =>
            {
                if (doc == null)
                {
                    throw new ModelNotFoundException(String.Format("The {0} document with Id {1} was not found.", documentTypeName, id));
                }
            };
        }

        /// <summary>
        /// Returns the configured batch size.
        /// </summary>
        /// <returns>The configured batch size.</returns>
        public int GetBatchSize()
        {
            return this.batchSize;
        }

        /// <summary>
        /// Returns a query to retrieves documents to process.
        /// </summary>
        /// <returns>The query to retrieve documents to process.</returns>
        public abstract IQueryable<TDocument> CreateGetDocumentsQuery();

        /// <summary>
        /// Returns a query to retrieve the document by id.
        /// </summary>
        /// <returns>The query to retrieve document by id to process.</returns>
        public abstract IQueryable<TDocument> CreateGetDocumentByIdQuery(object id);

        /// <summary>
        /// Returns a batch of documents to process.
        /// </summary>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="take">The number of records to return in the batch.</param>
        /// <returns>The batch of documents.</returns>
        public List<TDocument> GetDocumentBatch(int skip, int take)
        {
            return CreateGetDocumentsQuery().Skip(skip).Take(take).ToList();
        }

        /// <summary>
        /// Returns a batch of documents to process.
        /// </summary>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="take">The number of records to return in the batch.</param>
        /// <returns>The batch of documents.</returns>
        public Task<List<TDocument>> GetDocumentBatchAsync(int skip, int take)
        {
            return CreateGetDocumentsQuery().Skip(skip).Take(take).ToListAsync();
        }

        /// <summary>
        /// Returns the total number of documents to process.
        /// </summary>
        /// <returns>The total number of documents to process.</returns>
        public int GetDocumentCount()
        {
            return CreateGetDocumentsQuery().Count();
        }

        /// <summary>
        /// Returns the total number of documents to process.
        /// </summary>
        /// <returns>The total number of documents to process.</returns>
        public Task<int> GetDocumentCountAsync()
        {
            return CreateGetDocumentsQuery().CountAsync();
        }

        /// <summary>
        /// Indexes all documents via batches.
        /// </summary>
        public void AddOrUpdateAll()
        {
            var counter = 0;
            var total = GetDocumentCount();
            var config = indexService.GetDocumentConfiguration<TDocument>();
            throwIfDocumentConfigurationNotFound(config, typeof(TDocument));
            indexService.CreateIndex<TDocument>();
            var documentTypeName = config.GetDocumentTypeName();
            notificationService.StartedProcessingAllDocuments(documentTypeName);
            while (counter < total)
            {
                var documents = GetDocumentBatch(counter, batchSize);
                indexService.AddOrUpdate(documents);
                counter += documents.Count;
                notificationService.ProcessedSomeOfAllDocuments(documentTypeName, total, counter);
            }
            notificationService.ProcessAllDocumentsFinished(documentTypeName);
        }

        /// <summary>
        /// Indexes all documents via batches.
        /// </summary>
        public async Task AddOrUpdateAllAsync()
        {
            var counter = 0;
            var total = await GetDocumentCountAsync();
            var config = indexService.GetDocumentConfiguration<TDocument>();
            throwIfDocumentConfigurationNotFound(config, typeof(TDocument));
            await indexService.CreateIndexAsync<TDocument>();
            var documentTypeName = config.GetDocumentTypeName();
            notificationService.StartedProcessingAllDocuments(documentTypeName);
            while (counter < total)
            {
                var documents = await GetDocumentBatchAsync(counter, batchSize);
                await indexService.AddOrUpdateAsync(documents);
                counter += documents.Count;
                notificationService.ProcessedSomeOfAllDocuments(documentTypeName, total, counter);
            }
            notificationService.ProcessAllDocumentsFinished(documentTypeName);
        }

        /// <summary>
        /// Performs an add or update on a single document.
        /// </summary>
        /// <param name="id">The id of the entity whose document should be updated or added.</param>
        public void AddOrUpdateDocument(object id)
        {
            var config = indexService.GetDocumentConfiguration<TDocument>();
            throwIfDocumentConfigurationNotFound(config, typeof(TDocument));

            var documentTypeName = config.GetDocumentTypeName();
            notificationService.UpdateStarted(documentTypeName, id);

            var document = CreateGetDocumentByIdQuery(id).FirstOrDefault();
            throwIfDocumentNotFound(document, documentTypeName, id);
            
            indexService.AddOrUpdate(new List<TDocument> { document });
            notificationService.UpdateFinished(documentTypeName, config);
        }

        /// <summary>
        /// Performs an add or update on a single document.
        /// </summary>
        /// <param name="id">The id of the entity whose document should be updated or added.</param>
        public async Task AddOrUpdateDocumentAsync(object id)
        {
            var config = indexService.GetDocumentConfiguration<TDocument>();
            throwIfDocumentConfigurationNotFound(config, typeof(TDocument));
            
            var documentTypeName = config.GetDocumentTypeName();
            notificationService.UpdateStarted(documentTypeName, id);

            var document = await CreateGetDocumentByIdQuery(id).FirstOrDefaultAsync();
            throwIfDocumentNotFound(document, documentTypeName, id);
            
            await indexService.AddOrUpdateAsync(new List<TDocument> { document });
            notificationService.UpdateFinished(documentTypeName, config);
        }

        #region IDispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.indexService is IDisposable)
                {
                    (this.indexService as IDisposable).Dispose();
                    this.indexService = null;
                }
                if (this.notificationService is IDisposable)
                {
                    (this.notificationService as IDisposable).Dispose();
                    this.notificationService = null;
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        /// <summary>
        /// Returns the document type id for the documents this service will handle.
        /// </summary>
        /// <returns>The document type id for the documents this service will handle.</returns>
        public Guid GetDocumentTypeId()
        {
            var config = indexService.GetDocumentConfiguration<TDocument>();
            throwIfDocumentConfigurationNotFound(config, typeof(TDocument));
            return config.GetDocumentTypeId();
        }

        /// <summary>
        /// Deletes the documents whose entity ids are equal to the given ids.
        /// </summary>
        /// <param name="ids">The ids of the entities whose documents should be deleted.</param>
        public void DeleteDocuments(List<object> ids)
        {
            var config = indexService.GetDocumentConfiguration<TDocument>();
            throwIfDocumentConfigurationNotFound(config, typeof(TDocument));
            notificationService.DeleteDocumentsStarted(config.GetDocumentTypeName(), ids);
            indexService.DeleteDocuments(ids.Select(x => new DocumentKey(config.GetDocumentTypeId(), x)).ToList());
            notificationService.DeleteDocumentsFinished(config.GetDocumentTypeName(), ids);
        }

        /// <summary>
        /// Deletes the documents whose entity ids are equal to the given ids.
        /// </summary>
        /// <param name="ids">The ids of the entities whose documents should be deleted.</param>
        public async Task DeleteDocumentsAsync(List<object> ids)
        {
            var config = indexService.GetDocumentConfiguration<TDocument>();
            throwIfDocumentConfigurationNotFound(config, typeof(TDocument));
            notificationService.DeleteDocumentsStarted(config.GetDocumentTypeName(), ids);
            await indexService.DeleteDocumentsAsync(ids.Select(x => new DocumentKey(config.GetDocumentTypeId(), x)).ToList());
            notificationService.DeleteDocumentsFinished(config.GetDocumentTypeName(), ids);
        }
        
    }
    #endregion
}
