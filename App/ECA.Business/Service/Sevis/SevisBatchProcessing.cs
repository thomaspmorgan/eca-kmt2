using ECA.Business.Queries.Models.Sevis;
using ECA.Business.Queries.Sevis;
using ECA.Business.Service.Persons;
using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.Shared;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using System.Web.Script.Serialization;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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
        private IExchangeVisitorService exchangeVisitorService;

        /// <summary>
        /// Creates a new instance and initializes the context..
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="saveActions">The save actions.</param>
        public SevisBatchProcessingService(EcaContext context, IExchangeVisitorService exchangeVisitorService, ParticipantService participantService, ParticipantPersonsSevisService sevisService, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(exchangeVisitorService != null, "The exchange visitor service must not be null.");
            throwIfSevisBatchProcessingNotFound = (sevisBatchProcessing, batchId) =>
            {
                if (sevisBatchProcessing == null)
                {
                    throw new ModelNotFoundException(String.Format("The SEVIS batch processing record with the batch id [{0}] was not found.", batchId));
                }
            };
            this.exchangeVisitorService = exchangeVisitorService;
            this.participantService = participantService;
            this.sevisService = sevisService;
        }
        
        #region SEVIS validation

        /// <summary>
        /// Retrieve SEVIS batch XML
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="user"></param>
        /// <returns>XML of serialized sevis batch object</returns>
        public string GetSevisBatchCreateUpdateXML(int programId, User user)
        {
            // get sevis create objects
            var createEvs = GetSevisCreateEVs(user);

            // get sevis update objects
            var updateEvs = GetSevisUpdateEVs(user);

            // get sevis batch object
            var sevisBatch = CreateGetSevisBatchCreateUpdateEV(createEvs, updateEvs, programId, user);

            // get sevis xml
            var sevisXml = GetSevisBatchXml(sevisBatch);

            return sevisXml;
        }

        /// <summary>
        /// Retrieve a SEVIS batch to create/update exchange visitors
        /// </summary>
        /// <param name="createEVs"></param>
        /// <param name="updateEVs"></param>
        /// <param name="programId"></param>
        /// <param name="user"></param>
        /// <returns>Sevis batch object</returns>
        public SEVISBatchCreateUpdateEV CreateGetSevisBatchCreateUpdateEV(List<CreateExchVisitor> createEVs,
            List<UpdateExchVisitor> updateEVs, int programId, User user)
        {
            // create batch header
            var batchHeader = new BatchHeader
            {
                BatchID = DateTime.Today.ToString(),
                OrgID = programId.ToString()
            };
            var createEVBatch = new SEVISBatchCreateUpdateEV
            {
                userID = user.Id.ToString(),
                BatchHeader = batchHeader,
                UpdateEV = updateEVs,
                CreateEV = createEVs
            };

            return createEVBatch;
        }

        /// <summary>
        /// Retrieve participants with no sevis that are ready to submit
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Sevis exchange visitor create objects (250 max)</returns>
        public List<CreateExchVisitor> GetSevisCreateEVs(User user)
        {
            var participants = Context.ParticipantPersons
                                    .Where(x => x.ParticipantPersonSevisCommStatuses.Last().SevisCommStatusId == SevisCommStatus.ReadyToSubmit.Id && x.SevisId == null)
                                    .Select(x => new { ParticipantId = x.ParticipantId, ProjectId = x.Participant.ProjectId }).Take(250);

            List<CreateExchVisitor> createEvs = new List<CreateExchVisitor>();
            CreateExchVisitor createEv = new CreateExchVisitor();

            foreach (var participant in participants)
            {
                createEv = exchangeVisitorService.GetCreateExchangeVisitor(user, participant.ProjectId, participant.ParticipantId);
                createEvs.Add(createEv);
            }

            return createEvs;
        }

        /// <summary>
        /// Retrieve participants with sevis information that are ready to submit
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Sevis exchange visitor update objects (250 max)</returns>
        public List<UpdateExchVisitor> GetSevisUpdateEVs(User user)
        {
            var participants = Context.ParticipantPersons
                                    .Where(x => x.ParticipantPersonSevisCommStatuses.Last().SevisCommStatusId == SevisCommStatus.ReadyToSubmit.Id && x.SevisId == null)
                                    .Select(x => new { ParticipantId = x.ParticipantId, ProjectId = x.Participant.ProjectId }).Take(250);

            List<UpdateExchVisitor> updateEvs = new List<UpdateExchVisitor>();
            UpdateExchVisitor updateEv = new UpdateExchVisitor();

            foreach (var participant in participants)
            {
                updateEv = exchangeVisitorService.GetUpdateExchangeVisitor(user, participant.ProjectId, participant.ParticipantId);
                updateEvs.Add(updateEv);
            }

            return updateEvs;
        }

        /// <summary>
        /// Serialize SEVIS batch object to XML
        /// </summary>
        /// <param name="validationEntity">Participant object to be validated</param>
        /// <returns>XML of sevis batch object</returns>
        public string GetSevisBatchXml(SEVISBatchCreateUpdateEV validationEntity)
        {
            XmlSerializer serializer = new XmlSerializer(validationEntity.GetType());
            var settings = new XmlWriterSettings
            {
                NewLineHandling = NewLineHandling.Entitize,
                Encoding = System.Text.Encoding.UTF8,
                DoNotEscapeUriAttributes = true
            };
            using (var stream = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, validationEntity);
                    return stream.ToString();
                }
            }
        }

        #endregion

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
        public IEnumerable<SevisBatchProcessingDTO> GetSevisBatchesToUpload()
        {
            var dtos = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsForUpload(this.Context);
            logger.Trace("Retrieved the SEVIS Batch Processing dtos to upload");
            return dtos;
        }

        /// <summary>
        /// Retrieves the list of SEVIS Batch DTOs that have not been uploaded yet.
        /// </summary>
        /// <returns>The list of SevisBatchProcessing dtos.</returns>
        public Task<IEnumerable<SevisBatchProcessingDTO>> GetSevisBatchesToUploadAsync()
        {
            var dtos = Task.FromResult(SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsForUpload(this.Context));
            logger.Trace("Retrieved the SEVIS Batch Processing dtos to upload");
            return dtos;
        }

        /// <summary>
        ///  Retrieves the list of SEVIS Batch DTOs that have not been downloaded yet.
        /// </summary>
        /// <returns>The list of SevisBatchProcessing dtos.</returns>
        public IEnumerable<SevisBatchProcessingDTO> GetSevisBatchesToDownload()
        {
            var dtos = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsForDownload(this.Context);
            logger.Trace("Retrieved the SEVIS Batch Processing dtos to download");
            return dtos;
        }

        /// <summary>
        /// Retrieves the list of SEVIS Batch DTOs that have not been downloaded yet.
        /// </summary>
        /// <returns>The list of SevisBatchProcessing dtos.</returns>
        public Task<IEnumerable<SevisBatchProcessingDTO>> GetSevisBatchesToDownloadAsync()
        {
            var dtos = Task.FromResult(SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsForDownload(this.Context));
            logger.Trace("Retrieved the SEVIS Batch Processing dtos to download");
            return dtos;
        }

        /// <summary>
        ///  Retrieves the list of SEVIS Batch DTOs that have not been processed yet.
        /// </summary>
        /// <returns>The list of SevisBatchProcessing dtos.</returns>
        public IEnumerable<SevisBatchProcessingDTO> GetSevisBatchesToProcess()
        {
            var dtos = SevisBatchProcessingQueries.CreateGetSevisBatchProcessingDTOsToProcess(this.Context);
            logger.Trace("Retrieved the SEVIS Batch Processing dtos to download");
            return dtos;
        }

        /// <summary>
        /// Retrieves the list of SEVIS Batch DTOs that have not been processed yet.
        /// </summary>
        /// <returns>The list of SevisBatchProcessing dtos.</returns>
        public Task<IEnumerable<SevisBatchProcessingDTO>> GetSevisBatchesToProcessAsync()
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
            throwIfSevisBatchProcessingNotFound(newSevisBatchProcessing, newSevisBatchProcessing.BatchId);
            return newSevisBatchProcessing;
        }

        private async Task<SevisBatchProcessing> DoCreateAsync()
        {
            var newSevisBatchProcessing = new SevisBatchProcessing();
            await SaveChangesAsync();
            throwIfSevisBatchProcessingNotFound(newSevisBatchProcessing, newSevisBatchProcessing.BatchId);
            return newSevisBatchProcessing;
        }

        #endregion

        #region Update
        
        /// <summary>
        /// Process SEVIS batch transaction log
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="batchId">Batch ID</param>
        public async Task<IEnumerable<ParticipantSevisBatchProcessingResultDTO>> UpdateParticipantPersonSevisBatchStatusAsync(User user, int batchId)
        {
            var batchLog = await GetByIdAsync(batchId);
            var xml = batchLog.TransactionLogXml;
            var doc = XDocument.Parse(xml.ToString());
            List<ParticipantSevisBatchProcessingResultDTO> results = new List<ParticipantSevisBatchProcessingResultDTO>();
            List<ParticipantSevisBatchResultDTO> recordResults = new List<ParticipantSevisBatchResultDTO>();

            foreach (XElement recordElement in doc.Descendants("Record"))
            {
                var record = new ParticipantSevisBatchResultRecordDTO
                {
                    participantId = Convert.ToInt32(recordElement.Attribute("requestID").Value),
                    sevisId = recordElement.Attribute("sevisID").Value,
                    userId = Convert.ToInt32(recordElement.Attribute("userID").Value)
                };

                foreach (XElement resultElement in recordElement.Descendants("Result"))
                {
                    var result = new ParticipantSevisBatchResultDTO
                    {
                        statusCode = Convert.ToInt32(resultElement.Attribute("status").Value),
                        errorCode = resultElement.Descendants("ErrorCode").First().Value,
                        errorMessage = resultElement.Descendants("ErrorMessage").First().Value
                    };
                    recordResults.Add(result);
                }

                record.sevisResults = recordResults;
                results.Add(await UpdateParticipant(record));
            }

            return results;
        }

        /// <summary>
        /// Update a participant record with sevis batch results
        /// </summary>
        /// <returns></returns>
        private async Task<ParticipantSevisBatchProcessingResultDTO> UpdateParticipant(ParticipantSevisBatchResultRecordDTO record)
        {
            var result = new ParticipantSevisBatchProcessingResultDTO();
            var json = new JavaScriptSerializer().Serialize(record);
            var participantPersonDTO = await Context.ParticipantPersons.FindAsync(record.participantId);
            participantPersonDTO.SevisBatchResult = json;
            
            result.ParticipantId = record.participantId;
            result.ProjectId = participantPersonDTO.Participant.ProjectId;
            if (record.sevisResults.First().statusCode == 1)
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
    }
}
