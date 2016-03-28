using ECA.Business.Queries.Models.Sevis;
using ECA.Core.Service;
using ECA.Data;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ECA.Business.Service.Sevis
{
    /// <summary>
    /// An ISevisBatchProcessingService used to create or update SEVIS batch processing records.
    /// </summary>
    [ContractClass(typeof(SevisBatchProcessingServiceContract))]
    public interface ISevisBatchProcessingService : ISaveable
    {
        /// <summary>
        /// Returns the next batch record to send to sevis.
        /// </summary>
        /// <returns>Gets the next batch record to send to sevis.</returns>
        SevisBatchProcessingDTO GetNextBatchToUpload();

        /// <summary>
        /// Returns the next batch record to send to sevis.
        /// </summary>
        /// <returns>Gets the next batch record to send to sevis.</returns>
        Task<SevisBatchProcessingDTO> GetNextBatchToUploadAsync();

        /// <summary>
        /// Returns the batch id of the next batch to download from the sevis api.
        /// </summary>
        /// <returns>The batch id of the next batch to download from sevis.</returns>
        string GetNextBatchByBatchIdToDownload();

        /// <summary>
        /// Returns the batch id of the next batch to download from the sevis api.
        /// </summary>
        /// <returns>The batch id of the next batch to download from sevis.</returns>
        Task<string> GetNextBatchByBatchIdToDownloadAsync();

        /// <summary>
        /// Stages all queued to submit sevis participants into sevis batches that can then be sent to sevis for processing.
        /// </summary>
        /// <param name="user">The user performing the staging.</param>
        /// <returns>The list of staged sevis batches.</returns>
        List<StagedSevisBatch> StageBatches(User user);

        /// <summary>
        /// Stages all queued to submit sevis participants into sevis batches that can then be sent to sevis for processing.
        /// </summary>
        /// <param name="user">The user performing the staging.</param>
        /// <returns>The list of staged sevis batches.</returns>
        Task<List<StagedSevisBatch>> StageBatchesAsync(User user);

        /// <summary>
        /// Processes a given sevis transaction log as an xml string and updates system data appropriately.
        /// </summary>
        /// <param name="user">The user performing the processing.</param>
        /// <param name="xml">The sevis transaction log xml as a string.</param>
        void ProcessTransactionLog(User user, string xml);

        /// <summary>
        /// Processes a given sevis transaction log as an xml string and updates system data appropriately.
        /// </summary>
        /// <param name="user">The user performing the processing.</param>
        /// <param name="xml">The sevis transaction log xml as a string.</param>
        Task ProcessTransactionLogAsync(User user, string xml);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(ISevisBatchProcessingService))]
    public abstract class SevisBatchProcessingServiceContract : ISevisBatchProcessingService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetNextBatchByBatchIdToDownload()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<string> GetNextBatchByBatchIdToDownloadAsync()
        {
            return Task.FromResult<string>(null);
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SevisBatchProcessingDTO GetNextBatchToUpload()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<SevisBatchProcessingDTO> GetNextBatchToUploadAsync()
        {
            return Task.FromResult<SevisBatchProcessingDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        public void ProcessTransactionLog(string xml)
        {
            Contract.Requires(xml != null, "The xml must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="xml"></param>
        public void ProcessTransactionLog(User user, string xml)
        {
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(xml != null, "The xml must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public Task ProcessTransactionLogAsync(string xml)
        {
            Contract.Requires(xml != null, "The xml must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public Task ProcessTransactionLogAsync(User user, string xml)
        {
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(xml != null, "The xml must not be null.");
            return Task.FromResult<object>(null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult<int>(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<StagedSevisBatch> StageBatches(User user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<List<StagedSevisBatch>> StageBatchesAsync(User user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return Task.FromResult<List<StagedSevisBatch>>(null);
        }
    }
}
