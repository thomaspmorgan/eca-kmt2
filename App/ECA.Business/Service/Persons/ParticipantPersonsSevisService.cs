using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Business.Service.Sevis;
using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.Shared;
using ECA.Core.DynamicLinq;
using ECA.Core.Exceptions;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ECA.Business.Service.Persons
{

    /// <summary>
    /// A ParticipantPersonService is capable of performing crud operations on participantPersons in the ECA system.
    /// </summary>
    public class ParticipantPersonsSevisService : DbContextService<EcaContext>, IParticipantPersonsSevisService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<int, object, Type> throwIfModelDoesNotExist;
        private IParticipantPersonsSevisService participantService;

        /// <summary>
        /// Creates a new ParticipantPersonService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ParticipantPersonsSevisService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwIfModelDoesNotExist = (id, instance, type) =>
            {
                if (instance == null)
                {
                    throw new ModelNotFoundException(String.Format("The model of type [{0}] with id [{1}] was not found.", type.Name, id));
                }
            };
        }

        #region Get

        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        public PagedQueryResults<ParticipantPersonSevisDTO> GetParticipantPersonsSevis(QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            var participantPersonSevises = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons with query operator [{0}].", queryOperator);
            return participantPersonSevises;
        }

        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisDTO>> GetParticipantPersonsSevisAsync(QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            var participantPersonSevises = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons with query operator [{0}].", queryOperator);
            return participantPersonSevises;
        }

        /// <summary>
        /// Returns the participantPersonSevises for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersonSevises.</returns>
        public PagedQueryResults<ParticipantPersonSevisDTO> GetParticipantPersonsSevisByProjectId(int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            var participantPersonSevises = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantPersonSevises;
        }

        /// <summary>
        /// Returns the participantPersonSevises for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersonSevises.</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisDTO>> GetParticipantPersonsSevisByProjectIdAsync(int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            var participantPersonSevises = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantPersonSevises;
        }

        /// <summary>
        /// Returns a participantPersonSevis
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public ParticipantPersonSevisDTO GetParticipantPersonsSevisById(int participantId)
        {
            var participantPersonSevis = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevis;
        }

        /// <summary>
        /// Returns a participantPersonSevis asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public Task<ParticipantPersonSevisDTO> GetParticipantPersonsSevisByIdAsync(int participantId)
        {
            var participantPersonSevis = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(this.Context, participantId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevis;
        }

        #endregion

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
            var participantIds = Context.ParticipantPersons
                                    .Where(x => x.ParticipantPersonSevisCommStatuses.Last().SevisCommStatusId == SevisCommStatus.ReadyToSubmit.Id && x.SevisId == null)
                                    .Select(x => x.ParticipantId).Take(250);

            List<CreateExchVisitor> createEvs = new List<CreateExchVisitor>();
            CreateExchVisitor createEv = new CreateExchVisitor();

            foreach (var pid in participantIds)
            {
                createEv = ParticipantPersonsSevisQueries.GetCreateExchangeVisitor(pid, user, this.Context);
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
            var participantIds = Context.ParticipantPersons
                                    .Where(x => x.ParticipantPersonSevisCommStatuses.Last().SevisCommStatusId == SevisCommStatus.ReadyToSubmit.Id && x.SevisId == null)
                                    .Select(x => x.ParticipantId).Take(250);

            List<UpdateExchVisitor> updateEvs = new List<UpdateExchVisitor>();
            UpdateExchVisitor updateEv = new UpdateExchVisitor();

            foreach (var pid in participantIds)
            {
                updateEv = ParticipantPersonsSevisQueries.GetUpdateExchangeVisitor(pid, user, this.Context);
                updateEvs.Add(updateEv);
            }

            return updateEvs;
        }

        /// <summary>
        /// Update a participant SEVIS pre-validation status
        /// </summary>
        /// <param name="participantId">Participant ID</param>
        /// <param name="errorCount">Validation error count</param>
        /// <param name="isValid">Indicates if SEVIS object passed validation</param>
        /// <param name="result">Validation result object</param>
        public void UpdateParticipantPersonSevisCommStatus(int participantId, FluentValidation.Results.ValidationResult result)
        {
            var newStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participantId,
                AddedOn = DateTimeOffset.Now
            };

            if (result.Errors.Count > 0 || !result.IsValid)
            {
                newStatus.SevisCommStatusId = SevisCommStatus.InformationRequired.Id;
            }
            else
            {
                newStatus.SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id;
            }

            Context.ParticipantPersonSevisCommStatuses.Add(newStatus);
            Context.SaveChanges();            
        }

        /// <summary>
        /// Process SEVIS batch transaction log
        /// </summary>
        /// <param name="batchId">Batch ID</param>
        public async Task<int> UpdateParticipantPersonSevisBatchStatusAsync(User user, int batchId)
        {
            var service = new SevisBatchProcessingService(this.Context);
            var batchLog = await service.GetByIdAsync(batchId);
            int updates = 0;

            //StringBuilder sb = new StringBuilder();
            //sb.Append(@"<root><Process><Record sevisID=N0012309439 requestID=1179 userID=50>");
            //sb.Append(@"<Result><ErrorCode>S1056</ErrorCode><ErrorMessage>Invalid student visa type for this action</ErrorMessage></Result>");
            //sb.Append(@"</Record></Process></root>");
            
            var root = batchLog.TransactionLogXml;

            IEnumerable<XElement> participants =
                from el in root.Descendants("Process")
                where
                    (from record in el.Elements("Record")
                     select record).Any()
                select el;

            foreach (XElement record in participants)
            {
                var updatedParticipantPersonSevis = new UpdatedParticipantPersonSevis(user, (int)record.Attribute("UserID"), "", false, false, false, false, false, false, (DateTimeOffset?)record.Attribute("StartDate"), (DateTimeOffset?)record.Attribute("StartDate"), "");
                await participantService.UpdateAsync(updatedParticipantPersonSevis);
                await participantService.SaveChangesAsync();
                updates++;
            }

            return updates;
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

        #region ParticipantPersonSevisStatus

        /// Sevis Comm Status

        /// <summary>
        /// Returns the participantPersonSevisCommStatus in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        public PagedQueryResults<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonsSevisCommStatuses(QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            var participantPersonSevisCommStatuses = ParticipantPersonsSevisCommStatusQueries.CreateGetParticipantPersonsSevisCommStatusDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersonSevisCommStatuses with query operator [{0}].", queryOperator);
            return participantPersonSevisCommStatuses;
        }

        /// <summary>
        /// Returns the participantPersonSevisCommStatus in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> GetParticipantPersonsSevisCommStatusesAsync(QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            var participantPersonSevisCommStatuses = ParticipantPersonsSevisCommStatusQueries.CreateGetParticipantPersonsSevisCommStatusDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersonSevisCommStatuses with query operator [{0}].", queryOperator);
            return participantPersonSevisCommStatuses;
        }

        /// <summary>
        /// Returns a participantPersonSevis
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public PagedQueryResults<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonsSevisCommStatusesById(int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            var participantPersonSevisCommStatuses = ParticipantPersonsSevisCommStatusQueries.CreateGetParticipantPersonsSevisCommStatusDTOByIdQuery(this.Context, participantId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevisCommStatuses;
        }

        /// <summary>
        /// Returns a participantPersonSevisCommStatus asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> GetParticipantPersonsSevisCommStatusesByIdAsync(int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            var participantPersonSevisCommStatuses = ParticipantPersonsSevisCommStatusQueries.CreateGetParticipantPersonsSevisCommStatusDTOByIdQuery(this.Context, participantId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevisCommStatuses;
        }

        /// <summary>
        /// Returns a participantPersonSevisCommStatus asyncronously
        /// </summary>
        /// <param name="participantIds">The participant ids to lookup</param>
        /// <returns></returns>
        public IQueryable<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonsSevisCommStatusesByParticipantIds(int[] participantIds)
        {
            var results = ParticipantPersonsSevisCommStatusQueries.CreateParticipantPersonsSevisCommStatusesDTOsByParticipantIdsQuery(Context, participantIds);
            logger.Trace("Retrieved participantPersonSevises by array of participant Ids");
            return results;
        }

        #endregion

        #region update

        /// <summary>
        /// Sets sevis communication status for participant ids to queued
        /// </summary>
        /// <param name="participantIds">The participant ids to update communcation status</param>
        /// <returns>List of participant ids that were updated</returns>
        public async Task<int[]> SendToSevis(int[] participantIds)
        {
            var statuses = await Context.ParticipantPersonSevisCommStatuses.GroupBy(x => x.ParticipantId)
                .Select(s => s.OrderByDescending(x => x.AddedOn).FirstOrDefault())
                .Where(w => w.SevisCommStatusId == SevisCommStatus.ReadyToSubmit.Id && participantIds.Contains(w.ParticipantId))
                .ToListAsync();

            var participantsUpdated = new List<int>();

            foreach (var status in statuses)
            {
                var newStatus = new ParticipantPersonSevisCommStatus
                {
                    ParticipantId = status.ParticipantId,
                    SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                    AddedOn = DateTimeOffset.Now
                };

                Context.ParticipantPersonSevisCommStatuses.Add(newStatus);
                participantsUpdated.Add(status.ParticipantId);
            }

            return participantsUpdated.ToArray();
        }

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        public ParticipantPersonSevisDTO Update(UpdatedParticipantPersonSevis updatedParticipantPersonSevis)
        {
            var participantPerson = CreateGetParticipantPersonsByIdQuery(updatedParticipantPersonSevis.ParticipantId).FirstOrDefault();
            throwIfModelDoesNotExist(updatedParticipantPersonSevis.ParticipantId, participantPerson, typeof(ParticipantPerson));

            DoUpdate(participantPerson, updatedParticipantPersonSevis);
            return this.GetParticipantPersonsSevisById(updatedParticipantPersonSevis.ParticipantId);
        }

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        /// <returns>The task.</returns>
        public async Task<ParticipantPersonSevisDTO> UpdateAsync(UpdatedParticipantPersonSevis updatedParticipantPersonSevis)
        {
            var participantPerson = await CreateGetParticipantPersonsByIdQuery(updatedParticipantPersonSevis.ParticipantId).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(updatedParticipantPersonSevis.ParticipantId, participantPerson, typeof(ParticipantPerson));

            DoUpdate(participantPerson, updatedParticipantPersonSevis);

            return await this.GetParticipantPersonsSevisByIdAsync(updatedParticipantPersonSevis.ParticipantId);
        }

        private void DoUpdate(ParticipantPerson participantPerson, UpdatedParticipantPersonSevis updatedParticipantPersonSevis)
        {
            updatedParticipantPersonSevis.Audit.SetHistory(participantPerson);

            participantPerson.SevisId = updatedParticipantPersonSevis.SevisId;
            participantPerson.IsSentToSevisViaRTI = updatedParticipantPersonSevis.IsSentToSevisViaRTI;
            participantPerson.IsValidatedViaRTI = updatedParticipantPersonSevis.IsValidatedViaRTI;
            participantPerson.IsCancelled = updatedParticipantPersonSevis.IsCancelled;
            participantPerson.IsDS2019Printed = updatedParticipantPersonSevis.IsDS2019Printed;
            participantPerson.IsNeedsUpdate = updatedParticipantPersonSevis.IsNeedsUpdate;
            participantPerson.IsDS2019SentToTraveler = updatedParticipantPersonSevis.IsDS2019SentToTraveler;
            participantPerson.StartDate = updatedParticipantPersonSevis.StartDate;
            participantPerson.EndDate = updatedParticipantPersonSevis.EndDate;
            participantPerson.SevisValidationResult = updatedParticipantPersonSevis.SevisValidationResult;
        }

        private UpdatedParticipantPersonSevisValidationEntity GetUpdatedParticipantPersonSevisValidationEntity(ParticipantPerson participantPerson, UpdatedParticipantPersonSevis participantPersonSevis)
        {
            return new UpdatedParticipantPersonSevisValidationEntity(participantPerson, participantPersonSevis);
        }

        private IQueryable<ParticipantPerson> CreateGetParticipantPersonsByIdQuery(int participantId)
        {
            return Context.ParticipantPersons.Where(x => x.ParticipantId == participantId);
        }

        #endregion
    }
}
