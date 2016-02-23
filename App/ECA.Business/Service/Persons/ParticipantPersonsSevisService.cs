using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Validation.Model.Shared;
using ECA.Core.DynamicLinq;
using ECA.Core.Exceptions;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using Newtonsoft.Json;
using FluentValidation.Results;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
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
        private Action<int, int, Participant> throwSecurityViolationIfParticipantDoesNotBelongToProject;
        private IParticipantPersonsSevisService participantService;
        
        /// <summary>
        /// Creates a new ParticipantPersonService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ParticipantPersonsSevisService(EcaContext context, ISaveAction saveActions = null) : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwIfModelDoesNotExist = (id, instance, type) =>
            {
                if (instance == null)
                {
                    throw new ModelNotFoundException(String.Format("The model of type [{0}] with id [{1}] was not found.", type.Name, id));
                }
            };
            throwSecurityViolationIfParticipantDoesNotBelongToProject = (userId, projectId, participant) =>
        {
                if (participant != null && participant.ProjectId != projectId)
        {
                    throw new BusinessSecurityException(
                        String.Format("The user with id [{0}] attempted to validate a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        userId,
                        participant.ParticipantId,
                        projectId));
        }
            };
        }

        #region Get

        /// <summary>
        /// Returns a participantPersonSevis
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The participantPersonSevis</returns>
        public ParticipantPersonSevisDTO GetParticipantPersonsSevisById(int projectId, int participantId)
        {
            var participantPersonSevis = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(this.Context, projectId, participantId).FirstOrDefault();
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevis;
        }

        /// <summary>
        /// Returns a participantPersonSevis asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The participantPersonSevis</returns>
        public Task<ParticipantPersonSevisDTO> GetParticipantPersonsSevisByIdAsync(int projectId, int participantId)
        {
            var participantPersonSevis = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(this.Context, projectId, participantId).FirstOrDefaultAsync();
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
        public ParticipantPersonSevisCommStatus UpdateParticipantPersonSevisCommStatus(User user, int projectId, int participantId, FluentValidation.Results.ValidationResult result)
        {
            var participant = Context.Participants.Find(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            return DoUpdateParticipantPersonSevisCommStatus(user, projectId, participant, result);
        }

        /// <summary>
        /// Update a participant SEVIS pre-validation status
        /// </summary>
        /// <param name="participantId">Participant ID</param>
        /// <param name="projectId">The id of the project the participant belongs to.</param>
        /// <param name="user">The user performing the update.</param>
        /// <param name="result">Validation result object</param>
        /// <returns>The new comm status.</returns>
        public async Task<ParticipantPersonSevisCommStatus> UpdateParticipantPersonSevisCommStatusAsync(User user, int projectId, int participantId, FluentValidation.Results.ValidationResult result)
        {
            var participant = await Context.Participants.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            return DoUpdateParticipantPersonSevisCommStatus(user, projectId, participant, result);
        }

        private ParticipantPersonSevisCommStatus DoUpdateParticipantPersonSevisCommStatus(User user, int projectId, Participant participant, FluentValidation.Results.ValidationResult result)
        {
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);
            var newStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                AddedOn = DateTimeOffset.Now,
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
            return newStatus;
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

        #region update

        private IQueryable<ParticipantPersonSevisCommStatus> CreateGetCommStatusesThatAreReadyToSubmitQuery(int projectId, IEnumerable<int> participantIds)
        {
            var statuses = Context.ParticipantPersonSevisCommStatuses
                .Where(x => x.ParticipantPerson.Participant.ProjectId == projectId)
                .GroupBy(x => x.ParticipantId)
                .Select(s => s.OrderByDescending(x => x.AddedOn).FirstOrDefault())
                .Where(w => w.SevisCommStatusId == SevisCommStatus.ReadyToSubmit.Id && participantIds.Contains(w.ParticipantId));
            return statuses;
        }

        /// <summary>
        /// Sets sevis communication status for participant ids to queued
        /// </summary>
        /// <param name="participantIds">The participant ids to update communcation status</param>
        /// <returns>List of participant ids that were updated</returns>
        public async Task<int[]> SendToSevisAsync(int projectId, int[] participantIds)
        {
            var statuses = await CreateGetCommStatusesThatAreReadyToSubmitQuery(projectId, participantIds).ToListAsync();
            return DoSendToSevis(statuses).Select(x => x.ParticipantId).ToArray();
        }

        /// <summary>
        /// Sets sevis communication status for participant ids to queued
        /// </summary>
        /// <param name="participantIds">The participant ids to update communcation status</param>
        /// <returns>List of participant ids that were updated</returns>
        public int[] SendToSevis(int projectId, int[] participantIds)
        {
            var statuses = CreateGetCommStatusesThatAreReadyToSubmitQuery(projectId, participantIds).ToList();
            return DoSendToSevis(statuses).Select(x => x.ParticipantId).ToArray();
        }

        private IEnumerable<ParticipantPersonSevisCommStatus> DoSendToSevis(IEnumerable<ParticipantPersonSevisCommStatus> readyToSubmitStatuses)
        {
            var addedParticipantStatuses = new List<ParticipantPersonSevisCommStatus>();
            foreach (var status in readyToSubmitStatuses)
            {
                var newStatus = new ParticipantPersonSevisCommStatus
                {
                    ParticipantId = status.ParticipantId,
                    SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                    AddedOn = DateTimeOffset.Now
                };

                Context.ParticipantPersonSevisCommStatuses.Add(newStatus);
                addedParticipantStatuses.Add(status);
            }
            return addedParticipantStatuses.ToList();
        }

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        public void Update(UpdatedParticipantPersonSevis updatedParticipantPersonSevis)
        {
            var participantPerson = CreateGetParticipantPersonsByIdQuery(updatedParticipantPersonSevis.ParticipantId).FirstOrDefault();
            throwIfModelDoesNotExist(updatedParticipantPersonSevis.ParticipantId, participantPerson, typeof(ParticipantPerson));

            DoUpdate(participantPerson, updatedParticipantPersonSevis);
        }

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        /// <returns>The task.</returns>
        public async Task UpdateAsync(UpdatedParticipantPersonSevis updatedParticipantPersonSevis)
        {
            var participantPerson = await CreateGetParticipantPersonsByIdQuery(updatedParticipantPersonSevis.ParticipantId).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(updatedParticipantPersonSevis.ParticipantId, participantPerson, typeof(ParticipantPerson));

            DoUpdate(participantPerson, updatedParticipantPersonSevis);
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
