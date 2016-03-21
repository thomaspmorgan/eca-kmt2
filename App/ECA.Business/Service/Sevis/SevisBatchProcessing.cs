using ECA.Business.Queries.Sevis;
using ECA.Business.Queries.Models.Sevis;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using ECA.Business.Service.Persons;

namespace ECA.Business.Service.Sevis
{
    /// <summary>
    /// The SevisBatchProcessingService is capable of handling crud operations on a SevisBatchProcessingService record
    /// and its phone number.
    /// </summary>
    public class SevisBatchProcessingService : DbContextService<EcaContext>, ISevisBatchProcessingService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<SevisBatchProcessing, int> throwIfSevisBatchProcessingNotFound;
        private ParticipantService participantService;
        private ParticipantPersonsSevisService sevisService;

        /// <summary>
        /// Creates a new instance and initializes the context..
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="saveActions">The save actions.</param>
        public SevisBatchProcessingService(EcaContext context, ParticipantService participantService, ParticipantPersonsSevisService sevisService, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwIfSevisBatchProcessingNotFound = (sevisBatchProcessing, batchId) =>
            {
                if (sevisBatchProcessing == null)
                {
                    throw new ModelNotFoundException(String.Format("The SEVIS batch processing record with the batch id [{0}] was not found.", batchId));
                }
            };
            this.participantService = participantService;
            this.sevisService = sevisService;
        }

        #region Get
        /// <summary>
        /// Retrieves the SevisBatchProcessing dto with the given id.
        /// </summary>
        /// <param name="batchId">The id of the SEVIS Batch Processing record.</param>
        /// <returns>The phone number dto.</returns>
        public SevisBatchProcessingDTO GetById(int batchId)
        {
            var dto = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingByIdQuery(this.Context, batchId);
            logger.Info("Retrieved the Sevis Batch Processing dto with the given id [{0}].", batchId);
            return dto;
        }

        /// <summary>
        /// Retrieves the SevisBatchProcessing  dto with the given id.
        /// </summary>
        /// <param name="batchId">The id of the SevisBatchProcessing record.</param>
        /// <returns>The SevisBatchProcessing  dto.</returns>
        public Task<SevisBatchProcessingDTO> GetByIdAsync(int batchId)
        {
            var dto = Task.FromResult(SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOByIdQuery(this.Context, batchId));
            logger.Info("Retrieved the Sevis Batch Processing dto with the given id [{0}].", batchId);
            return dto;
        }

        /// <summary>
        ///  Retrieves the list of SEVIS Batch DTOs that have not been uploaded yet.
        /// </summary>
        /// <returns>The list of SevisBatchProcessing dtos.</returns>
        public IQueryable<SevisBatchProcessingDTO> GetSevisBatchesToUpload()
        {
            var dtos = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsForUpload(this.Context);
            logger.Trace("Retrieved the SEVIS Batch Processing dtos to upload");
            return dtos;
        }

        /// <summary>
        /// Retrieves the list of SEVIS Batch DTOs that have not been uploaded yet.
        /// </summary>
        /// <returns>The list of SevisBatchProcessing dtos.</returns>
        public Task<IQueryable<SevisBatchProcessingDTO>> GetSevisBatchesToUploadAsync()
        {
            var dtos = Task.FromResult(SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsForUpload(this.Context));
            logger.Trace("Retrieved the SEVIS Batch Processing dtos to upload");
            return dtos;
        }

        /// <summary>
        ///  Retrieves the list of SEVIS Batch DTOs that have not been downloaded yet.
        /// </summary>
        /// <returns>The list of SevisBatchProcessing dtos.</returns>
        public IQueryable<SevisBatchProcessingDTO> GetSevisBatchesToDownload()
        {
            var dtos = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsForDownload(this.Context);
            logger.Trace("Retrieved the SEVIS Batch Processing dtos to download");
            return dtos;
        }

        /// <summary>
        /// Retrieves the list of SEVIS Batch DTOs that have not been downloaded yet.
        /// </summary>
        /// <returns>The list of SevisBatchProcessing dtos.</returns>
        public Task<IQueryable<SevisBatchProcessingDTO>> GetSevisBatchesToDownloadAsync()
        {
            var dtos = Task.FromResult(SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsForDownload(this.Context));
            logger.Trace("Retrieved the SEVIS Batch Processing dtos to download");
            return dtos;
        }

        /// <summary>
        ///  Retrieves the list of SEVIS Batch DTOs that have not been processed yet.
        /// </summary>
        /// <returns>The list of SevisBatchProcessing dtos.</returns>
        public IQueryable<SevisBatchProcessingDTO> GetSevisBatchesToProcess()
        {
            var dtos = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToProcess(this.Context);
            logger.Trace("Retrieved the SEVIS Batch Processing dtos to download");
            return dtos;
        }

        /// <summary>
        /// Retrieves the list of SEVIS Batch DTOs that have not been processed yet.
        /// </summary>
        /// <returns>The list of SevisBatchProcessing dtos.</returns>
        public Task<IQueryable<SevisBatchProcessingDTO>> GetSevisBatchesToProcessAsync()
        {
            var dtos = Task.FromResult(SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToProcess(this.Context));
            logger.Trace("Retrieved the SEVIS Batch Processing dtos to download");
            return dtos;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a new SEVIS Batch Processing record in the ECA system.
        /// </summary>
        /// <returns>The created SEVIS Batch Processing entity.</returns>
        public SevisBatchProcessing Create()
        {
            return DoCreate();
        }

        /// <summary>
        /// Creates a new Sevis Batch Processing record in the ECA system.
        /// </summary>
        /// <returns>The created  phone number entity.</returns>
        public async Task<SevisBatchProcessing> CreateAsync()
        {
            return await DoCreateAsync();
        }

        private SevisBatchProcessing DoCreate() 
        {
            var newSevisBatchProcessing = new SevisBatchProcessing();
            SaveChanges();
            return newSevisBatchProcessing;
        }

        private async Task<SevisBatchProcessing> DoCreateAsync()
        {
            var newSevisBatchProcessing = new SevisBatchProcessing();
            await SaveChangesAsync();
            return newSevisBatchProcessing;
        }

        #endregion

        #region Update
        
        /// <summary>
        /// Process SEVIS batch transaction log
        /// </summary>
        /// <param name="batchId">Batch ID</param>
        public async Task<IQueryable<ParticipantSevisBatchProcessingResultDTO>> UpdateParticipantPersonSevisBatchStatusAsync(User user, int batchId)
        {
            var batchLog = await GetByIdAsync(batchId);
            var xml = batchLog.TransactionLogXml;
            var doc = XDocument.Parse(xml.ToString());
            List<ParticipantSevisBatchProcessingResultDTO> results = new List<ParticipantSevisBatchProcessingResultDTO>();

            foreach (XElement record in doc.Descendants("Record"))
            {
                var participantID = Convert.ToInt32(record.Attribute("requestID").Value);
                var status = record.Descendants("Result").First().Attribute("status").Value;
                string json = JsonConvert.SerializeXNode(record);

                // update participant person batch result
                results.Add(await UpdateParticipant(participantID, status, json));
            }

            return results.AsQueryable();
        }

        /// <summary>
        /// Update a participant record with sevis batch results
        /// </summary>
        /// <param name="participantID"></param>
        /// <param name="status"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        private async Task<ParticipantSevisBatchProcessingResultDTO> UpdateParticipant(int participantID, string status, string json)
        {
            var result = new ParticipantSevisBatchProcessingResultDTO();
            
            var participantPersonDTO = await Context.ParticipantPersons.FindAsync(participantID);
            participantPersonDTO.SevisBatchResult = json;
            
            result.ParticipantId = participantID;
            result.ProjectId = participantPersonDTO.Participant.ProjectId;
            if (status == "1")
            {
                result.SevisCommStatus = SevisCommStatus.BatchRequestSuccessful.Value;
            }
            else
            {
                result.SevisCommStatus = SevisCommStatus.BatchRequestUnsuccessful.Value;
            }

            return result;
        }

        /// <summary>
        /// Updates the ECA system's SEVIS Batch Processing record with the given updated SEVIS Batch Processing record.
        /// </summary>
        /// <param name="updatedSevisBatchProcessing">The updated SEVIS Batch Processing record.</param>
        public void Update(UpdatedSevisBatchProcessing updatedSevisBatchProcessing)
        {
            var sevisBatchProcessing = Context.SevisBatchProcessings.Find(updatedSevisBatchProcessing.BatchId);
            DoUpdate(updatedSevisBatchProcessing, sevisBatchProcessing);
        }

        /// <summary>
        /// Updates the ECA system's SEVIS Batch Processing record with the given SEVIS Batch Processing record.
        /// </summary>
        /// <param name="updatedSevisBatchProcessing">The SEVIS Batch Processing record.</param>
        public async Task UpdateAsync(UpdatedSevisBatchProcessing updatedSevisBatchProcessing)
        {
            var sevisBatchProcessing = await Context.SevisBatchProcessings.FindAsync(updatedSevisBatchProcessing.BatchId);
            DoUpdate(updatedSevisBatchProcessing, sevisBatchProcessing);
        }

        private void DoUpdate(UpdatedSevisBatchProcessing updatedSevisBatchProcessing, SevisBatchProcessing modelToUpdate)
        {
            Contract.Requires(updatedSevisBatchProcessing != null, "The updatedSevisBatchProcessing must not be null.");
            throwIfSevisBatchProcessingNotFound(modelToUpdate, updatedSevisBatchProcessing.BatchId);
            modelToUpdate.SubmitDate = updatedSevisBatchProcessing.SubmitDate;
            modelToUpdate.RetrieveDate = updatedSevisBatchProcessing.RetrieveDate;
            modelToUpdate.SendXml = updatedSevisBatchProcessing.SendXml;
            modelToUpdate.TransactionLogXml = updatedSevisBatchProcessing.TransactionLogXml;
            modelToUpdate.DownloadDispositionCode = updatedSevisBatchProcessing.DownloadDispositionCode;
            modelToUpdate.ProcessDispositionCode = updatedSevisBatchProcessing.ProcessDispositionCode;
            modelToUpdate.UploadDispositionCode = updatedSevisBatchProcessing.UploadDispositionCode;
        }

        #endregion

        #region Delete
        /// <summary>
        /// Deletes the SEVIS Batch Processing record with the given id.
        /// </summary>
        /// <param name="batchId">The id of the SEVIS Batch Processing record to delete.</param>
        public void Delete(int batchId)
        {
            var sevisBatchProcessing = Context.SevisBatchProcessings.Find(batchId);
            DoDelete(sevisBatchProcessing);
        }

        /// <summary>
        /// Deletes the SEVIS Batch Processing record with the given id.
        /// </summary>
        /// <param name="phoneNumberId">The id of the SEVIS Batch Processing record to delete.</param>
        public async Task DeleteAsync(int batchId)
        {
            var sevisBatchProcessing = await Context.SevisBatchProcessings.FindAsync(batchId);
            DoDelete(sevisBatchProcessing);
        }

        private void DoDelete(SevisBatchProcessing sevisBatchProcessingToDelete)
        {
            if (sevisBatchProcessingToDelete != null)
            {
                Context.SevisBatchProcessings.Remove(sevisBatchProcessingToDelete);
            }
        }

        #endregion


        public string GetSevisBatchCreateUpdateXML(int programId, User user)
        {
            throw new NotImplementedException();
        }
    }
}
