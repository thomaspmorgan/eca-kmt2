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
        ///// <summary>
        ///// Creates a new SevisBatchProcessing record in the ECA system.
        ///// </summary>
        ///// <returns>The created  SevisBatchProcessingr entity.</returns>
        //SevisBatchProcessing Create();

        ///// <summary>
        ///// Creates a  new SevisBatchProcessing record in the ECA system.
        ///// </summary>
        ///// <returns>The created SevisBatchProcessing entity.</returns>
        //Task<SevisBatchProcessing> CreateAsync();

        ///// <summary>
        ///// Updates the ECA system's SevisBatchProcessing data with the given updated SevisBatchProcessing record.
        ///// </summary>
        ///// <param name="updatedSevisBatchProcessing">The updated SevisBatchProcessing record.</param>
        //void Update(UpdatedSevisBatchProcessing updatedSevisBatchProcessing);

        ///// <summary>
        ///// Updates the ECA system's SevisBatchProcessing record with the given SevisBatchProcessing record.
        ///// </summary>
        ///// <param name="updatedSevisBatchProcessing">The SevisBatchProcessing record.</param>
        //Task UpdateAsync(UpdatedSevisBatchProcessing updatedSevisBatchProcessing);

        ///// <summary>
        ///// Process SEVIS batch transaction log
        ///// </summary>
        ///// <param name="batchId">Batch ID</param>
        ///// <param name="user">User</param>
        //Task<IQueryable<ParticipantSevisBatchProcessingResultDTO>> UpdateParticipantPersonSevisBatchStatusAsync(User user, int batchId);

        ///// <summary>
        ///// Retrieves the  SevisBatchProcessing dto with the given id.
        ///// </summary>
        ///// <param name="id">The id of the SevisBatchProcessing.</param>
        ///// <returns>The  SevisBatchProcessing dto.</returns>
        //SevisBatchProcessingDTO GetById(int id);

        ///// <summary>
        ///// Retrieves the phone number dto with the given id.
        ///// </summary>
        ///// <param name="id">The id of the SevisBatchProcessing record.</param>
        ///// <returns>The SevisBatchProcessing dto.</returns>
        //Task<SevisBatchProcessingDTO> GetByIdAsync(int id);

        ///// <summary>
        /////  Retrieves the list of SEVIS Batch DTOs that have not been uploaded yet.
        ///// </summary>
        ///// <returns>The list of SevisBatchProcessing dtos.</returns>
        //IQueryable<SevisBatchProcessingDTO> GetSevisBatchesToUpload();

        ///// <summary>
        ///// Retrieves the list of SEVIS Batch DTOs that have not been uploaded yet.
        ///// </summary>
        ///// <returns>The list of SevisBatchProcessing dtos.</returns>
        //Task<IQueryable<SevisBatchProcessingDTO>> GetSevisBatchesToUploadAsync();

        ///// <summary>
        ///// Deletes the SevisBatchProcessing entry with the given id.
        ///// </summary>
        ///// <param name="batchId">The id of the SevisBatchProcessing to delete.</param>
        //void Delete(int batchId);

        ///// <summary>
        ///// Deletes the SevisBatchProcessing entry with the given id.
        ///// </summary>
        ///// <param name="batchId">The id of the SevisBatchProcessing record to delete.</param>
        //Task DeleteAsync(int batchId);

        ///// <summary>
        ///// Retrieve SEVIS batch XML
        ///// </summary>
        ///// <param name="programId"></param>
        ///// <param name="user"></param>
        ///// <returns></returns>
        //string GetSevisBatchCreateUpdateXML(int programId, User user);

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
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(ISevisBatchProcessingService))]
    public abstract class SevisBatchProcessingServiceContract : ISevisBatchProcessingService
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public SevisBatchProcessing Create()
        //{
        //    Contract.Ensures(Contract.Result<SevisBatchProcessing>() != null, "The SEVIS batch processing entity returned must not be null.");
        //    return null;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="newSevisBatchProcessing"></param>
        ///// <returns></returns>
        //public Task<SevisBatchProcessing> CreateAsync()
        //{
        //    Contract.Ensures(Contract.Result<Task<SevisBatchProcessing>>() != null, "The SEVIS batch processing entity returned must not be null.");
        //    return Task.FromResult<SevisBatchProcessing>(null);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="updatedSevisBatchProcessing"></param>
        //public void Update(UpdatedSevisBatchProcessing updatedSevisBatchProcessing)
        //{
        //    Contract.Requires(updatedSevisBatchProcessing != null, "The updated SEVIS batch processing record must not be null.");
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="updatedSevisBatchProcessing"></param>
        ///// <returns></returns>
        //public Task UpdateAsync(UpdatedSevisBatchProcessing updatedSevisBatchProcessing)
        //{
        //    Contract.Requires(updatedSevisBatchProcessing != null, "The updated SEVIS batch processing must not be null.");
        //    return Task.FromResult<object>(null);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="batchId"></param>
        ///// <returns></returns>
        //public Task<IQueryable<ParticipantSevisBatchProcessingResultDTO>> UpdateParticipantPersonSevisBatchStatusAsync(User user, int batchId)
        //{
        //    Contract.Requires(user != null, "The user must not be null.");
        //    return Task.FromResult<IQueryable<ParticipantSevisBatchProcessingResultDTO>>(null);
        //}

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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public SevisBatchProcessingDTO GetById(int id)
        //{
        //    return null;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Task<SevisBatchProcessingDTO> GetByIdAsync(int id)
        //{
        //    return Task.FromResult<SevisBatchProcessingDTO>(null);
        //}

        ///// <summary>
        /////  Retrieves the list of SEVIS Batch DTOs that have not been uploaded yet.
        ///// </summary>
        ///// <returns>The list of SevisBatchProcessing dtos.</returns>
        //public IQueryable<SevisBatchProcessingDTO> GetSevisBatchesToUpload()
        //{
        //    return null;
        //}

        ///// <summary>
        ///// Retrieves the list of SEVIS Batch DTOs that have not been uploaded yet.
        ///// </summary>
        ///// <returns>The list of SevisBatchProcessing dtos.</returns>
        //public Task<IQueryable<SevisBatchProcessingDTO>> GetSevisBatchesToUploadAsync()
        //{
        //    return Task.FromResult<IQueryable<SevisBatchProcessingDTO>>(null);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="batchId"></param>
        //public void Delete(int batchId)
        //{

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="batchId"></param>
        ///// <returns></returns>
        //public Task DeleteAsync(int batchId)
        //{
        //    return Task.FromResult<object>(null);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="programId"></param>
        ///// <param name="user"></param>
        ///// <returns></returns>
        //public string GetSevisBatchCreateUpdateXML(int programId, User user)
        //{
        //    return string.Empty;
        //}
    }
}
