﻿using ECA.Core.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// An IDocumentService is capable of retrieving and indexing documents from a datastore.
    /// </summary>
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
        /// Processes the documents.
        /// </summary>
        void Process();

        /// <summary>
        /// Processes the documents.
        /// </summary>
        Task ProcessAsync();
    }

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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TDocument"></typeparam>
    [ContractClassFor(typeof(DocumentService<,>))]
    public abstract class DocumentServiceContract<TContext, TDocument>  : DocumentService<TContext, TDocument>
        where TContext : DbContext
        where TDocument : class
    {
        public DocumentServiceContract(TContext context, IIndexService indexService, IIndexNotificationService notificationService)
            :base(context, indexService, notificationService)
        {

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

        public List<TDocument> GetDocumentBatch(int skip, int take)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public Task<List<TDocument>> GetDocumentBatchAsync(int skip, int take)
        {
            return Task.FromResult<List<TDocument>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetDocumentCount()
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<int> GetDocumentCountAsync()
        {
            return Task.FromResult<int>(0);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Process()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task ProcessAsync()
        {
            return Task.FromResult<object>(null);
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
        public void Process()
        {
            var counter = 0;
            var total = GetDocumentCount();
            var config = indexService.GetDocumentConfiguration<TDocument>();
            throwIfDocumentConfigurationNotFound(config, typeof(TDocument));
            indexService.CreateIndex<TDocument>();
            var documentTypeName = config.GetDocumentTypeName();
            notificationService.Started(documentTypeName);
            while (counter < total)
            {
                var documents = GetDocumentBatch(counter, batchSize);
                indexService.HandleDocuments(documents);                
                counter += documents.Count;
                notificationService.Processed(documentTypeName, total, counter);
            }
            notificationService.Finished(documentTypeName);
        }

        /// <summary>
        /// Indexes all documents via batches.
        /// </summary>
        public async Task ProcessAsync()
        {
            var counter = 0;
            var total = await GetDocumentCountAsync();
            var config = indexService.GetDocumentConfiguration<TDocument>();
            throwIfDocumentConfigurationNotFound(config, typeof(TDocument));
            await indexService.CreateIndexAsync<TDocument>();
            var documentTypeName = config.GetDocumentTypeName();
            notificationService.Started(documentTypeName);
            while (counter < total)
            {
                var documents = await GetDocumentBatchAsync(counter, batchSize);
                await indexService.HandleDocumentsAsync(documents);                
                counter += documents.Count;
                notificationService.Processed(documentTypeName, total, counter);
            }
            notificationService.Finished(documentTypeName);
        }


        #region IDispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(this.indexService is IDisposable)
                {
                    (this.indexService as IDisposable).Dispose();
                    this.indexService = null;
                }
                if(this.notificationService is IDisposable)
                {
                    (this.notificationService as IDisposable).Dispose();
                    this.notificationService = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
