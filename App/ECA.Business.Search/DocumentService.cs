using ECA.Core.Service;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public interface IDocumentService<TDocument> where TDocument : class
    {
        IQueryable<TDocument> CreateGetDocumentsQuery();

        int GetDocumentCount();

        Task<int> GetDocumentCountAsync();

        List<TDocument> GetDocumentBatch(int skip, int take);

        Task<List<TDocument>> GetDocumentBatchAsync(int skip, int take);

        void Process();

        Task ProcessAsync();
    }

    public abstract class DocumentService<TContext, TDocument> : DbContextService<TContext>, IDocumentService<TDocument>
        where TContext : DbContext
        where TDocument : class
    {
        public const int DEFAULT_BATCH_SIZE = 500;

        private int batchSize;
        private IIndexService indexService;
        private IIndexNotificationService notificationService;
        private Action<IDocumentConfiguration, Type> throwIfDocumentConfigurationNotFound;

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

        public abstract IQueryable<TDocument> CreateGetDocumentsQuery();

        public List<TDocument> GetDocumentBatch(int skip, int take)
        {
            return CreateGetDocumentsQuery().Skip(skip).Take(take).ToList();
        }

        public Task<List<TDocument>> GetDocumentBatchAsync(int skip, int take)
        {
            return CreateGetDocumentsQuery().Skip(skip).Take(take).ToListAsync();
        }

        public int GetDocumentCount()
        {
            return CreateGetDocumentsQuery().Count();
        }

        public Task<int> GetDocumentCountAsync()
        {
            return CreateGetDocumentsQuery().CountAsync();
        }

        public void Process()
        {
            var counter = 0;
            var total = GetDocumentCount();
            var config = indexService.GetDocumentConfiguration<TDocument>();
            throwIfDocumentConfigurationNotFound(config, typeof(TDocument));
            var documentType = config.GetDocumentType();
            while (counter < total)
            {
                var documents = GetDocumentBatch(counter, batchSize).ToList();
                indexService.HandleDocuments(documents);
                notificationService.Processed(documentType, total, counter);
                counter += documents.Count;
            }
            notificationService.Finished(documentType);
        }

        public async Task ProcessAsync()
        {
            var counter = 0;
            var total = await GetDocumentCountAsync();
            var config = indexService.GetDocumentConfiguration<TDocument>();
            throwIfDocumentConfigurationNotFound(config, typeof(TDocument));
            var documentType = config.GetDocumentType();
            while (counter < total)
            {
                var documents = (await GetDocumentBatchAsync(counter, batchSize)).ToList();
                indexService.HandleDocuments(documents);
                notificationService.Processed(documentType, total, counter);
                counter += documents.Count;
            }
            notificationService.Finished(documentType);
        }
    }
}
