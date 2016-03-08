using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// The ExchangeVisitorValidationService is used to execute sevis validation on a participant and save the validation results
    /// and sevis comm status to the datastore.
    /// </summary>
    public class ExchangeVisitorValidationService : EcaService, IExchangeVisitorValidationService
    {
        private readonly Action<int, object, Type> throwIfModelDoesNotExist;
        private readonly Action<int, int, Participant> throwSecurityViolationIfParticipantDoesNotBelongToProject;
        private IExchangeVisitorService exchangeVisitorService;

        /// <summary>
        /// Creates a new instance of the service with the context to query and the validators.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="exchangeVisitorService">The exchange visitor service that is capable of executing validation.</param>
        /// <param name="updateExchVisitorValidator">The update exchange visitor validator.</param>
        /// <param name="createExchVisitorValidator">The create exchange visitor validator.</param>
        /// <param name="saveActions">The context save actions.</param>
        public ExchangeVisitorValidationService(EcaContext context,
            IExchangeVisitorService exchangeVisitorService,
            AbstractValidator<UpdateExchVisitor> updateExchVisitorValidator = null,
            AbstractValidator<CreateExchVisitor> createExchVisitorValidator = null,
            List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(exchangeVisitorService != null, "The exchange visitor service must not be null.");
            Contract.Requires(context != null, "The context must not be null.");
            this.exchangeVisitorService = exchangeVisitorService;
            this.UpdateExchangeVisitorValidator = updateExchVisitorValidator ?? new UpdateExchVisitorValidator();
            this.CreateExchangeVisitorValidator = createExchVisitorValidator ?? new CreateExchVisitorValidator();

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

        /// <summary>
        /// Gets the update exchange visitor fluent validation validator.
        /// </summary>
        public AbstractValidator<UpdateExchVisitor> UpdateExchangeVisitorValidator { get; private set; }

        /// <summary>
        /// Gets the create exchange visitor fluent validation validator.
        /// </summary>
        public AbstractValidator<CreateExchVisitor> CreateExchangeVisitorValidator { get; private set; }

        /// <summary>
        /// Runs a validation on sevis information for the participant with the given id and updates the sevis comm
        /// status with the latest validation result.  If the sevis validation is successful then a sevis comm status is added
        /// with the Ready To Submit status, otherwise, Information Required is set.
        /// </summary>
        /// <param name="user">The user performing the validation.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The id of the participant.</param>
        /// <returns>The added participant sevis comm status.</returns>
        public ParticipantPersonSevisCommStatus RunParticipantSevisValidation(User user, int projectId, int participantId)
        {
            var participant = Context.Participants.Find(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);

            var participantPerson = Context.ParticipantPersons.Find(participantId);
            throwIfModelDoesNotExist(participantId, participantPerson, typeof(ParticipantPerson));

            var project = Context.Projects.Find(projectId);
            throwIfModelDoesNotExist(participantId, project, typeof(Project));
            if (project.VisitorTypeId == VisitorType.ExchangeVisitor.Id && participant.ParticipantTypeId == ParticipantType.ForeignTravelingParticipant.Id)
            {
                ValidationResult validationResult;
                if (!String.IsNullOrWhiteSpace(participantPerson.SevisId))
                {
                    var updateExchangeVisitor = exchangeVisitorService.GetUpdateExchangeVisitor(user, projectId, participantId);
                    validationResult = this.UpdateExchangeVisitorValidator.Validate(updateExchangeVisitor);
                }
                else
                {
                    var createExchangeVisitor = exchangeVisitorService.GetCreateExchangeVisitor(user, projectId, participantId);
                    validationResult = this.CreateExchangeVisitorValidator.Validate(createExchangeVisitor);
                }
                return HandleValidationResult(participantPerson, validationResult);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Runs a validation on sevis information for the participant with the given id and updates the sevis comm
        /// status with the latest validation result.  If the sevis validation is successful then a sevis comm status is added
        /// with the Ready To Submit status, otherwise, Information Required is set.
        /// </summary>
        /// <param name="user">The user performing the validation.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The id of the participant.</param>
        /// <returns>The added participant sevis comm status.</returns>
        public async Task<ParticipantPersonSevisCommStatus> RunParticipantSevisValidationAsync(User user, int projectId, int participantId)
        {
            var participant = await Context.Participants.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);

            var participantPerson = await Context.ParticipantPersons.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participantPerson, typeof(ParticipantPerson));

            var project = await Context.Projects.FindAsync(projectId);
            throwIfModelDoesNotExist(participantId, project, typeof(Project));
            if (project.VisitorTypeId == VisitorType.ExchangeVisitor.Id && participant.ParticipantTypeId == ParticipantType.ForeignTravelingParticipant.Id)
            {
                ValidationResult validationResult;
                if (!String.IsNullOrWhiteSpace(participantPerson.SevisId))
                {
                    var updateExchangeVisitor = await exchangeVisitorService.GetUpdateExchangeVisitorAsync(user, projectId, participantId);
                    validationResult = this.UpdateExchangeVisitorValidator.Validate(updateExchangeVisitor);
                }
                else
                {
                    var createExchangeVisitor = await exchangeVisitorService.GetCreateExchangeVisitorAsync(user, projectId, participantId);
                    validationResult = this.CreateExchangeVisitorValidator.Validate(createExchangeVisitor);
                }
                return await HandleValidationResultAsync(participantPerson, validationResult);
            }
            else
            {
                return null;
            }
        }

        private async Task<ParticipantPersonSevisCommStatus> HandleValidationResultAsync(ParticipantPerson person, ValidationResult result)
        {
            if (!result.IsValid)
            {
                person.SevisValidationResult = JsonConvert.SerializeObject(
                    new SimpleValidationResult(result),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                var latestCommStatus = await CreateGetLatestParticipantPersonSevisCommStatusQuery(person.ParticipantId).FirstOrDefaultAsync();
                return AddOrUpdateParticipantPersonSevisCommStatus(latestCommStatus, person.ParticipantId, SevisCommStatus.InformationRequired.Id);

            }
            else
            {
                person.SevisValidationResult = null;
                return AddParticipantPersonSevisCommStatus(person.ParticipantId, SevisCommStatus.ReadyToSubmit.Id);
            }
        }

        private ParticipantPersonSevisCommStatus HandleValidationResult(ParticipantPerson person, ValidationResult result)
        {
            if (!result.IsValid)
            {
                person.SevisValidationResult = JsonConvert.SerializeObject(
                    new SimpleValidationResult(result),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                var latestCommStatus = CreateGetLatestParticipantPersonSevisCommStatusQuery(person.ParticipantId).FirstOrDefault();
                return AddOrUpdateParticipantPersonSevisCommStatus(latestCommStatus, person.ParticipantId, SevisCommStatus.InformationRequired.Id);
            }
            else
            {
                person.SevisValidationResult = null;
                return AddParticipantPersonSevisCommStatus(person.ParticipantId, SevisCommStatus.ReadyToSubmit.Id);
            }
        }

        private IQueryable<ParticipantPersonSevisCommStatus> CreateGetLatestParticipantPersonSevisCommStatusQuery(int participantId)
        {
            return Context.ParticipantPersonSevisCommStatuses.Where(x => x.ParticipantId == participantId).OrderByDescending(x => x.AddedOn);
        }

        private ParticipantPersonSevisCommStatus AddParticipantPersonSevisCommStatus(int participantId, int commStatusId)
        {
            var status = new ParticipantPersonSevisCommStatus
            {
                AddedOn = DateTimeOffset.UtcNow,
                ParticipantId = participantId,
                SevisCommStatusId = commStatusId
            };
            this.Context.ParticipantPersonSevisCommStatuses.Add(status);
            return status;
        }

        private ParticipantPersonSevisCommStatus AddOrUpdateParticipantPersonSevisCommStatus(ParticipantPersonSevisCommStatus latestStatus, int participantId, int commStatusId)
        {
            if (latestStatus == null)
            {
                return AddParticipantPersonSevisCommStatus(participantId, commStatusId);
            }
            else
            {
                if (latestStatus.SevisCommStatusId == SevisCommStatus.InformationRequired.Id)
                {
                    latestStatus.AddedOn = DateTimeOffset.UtcNow;
                    return latestStatus;
                }
                else
                {
                    return AddParticipantPersonSevisCommStatus(participantId, commStatusId);
                }
            }
        }
    }
}
