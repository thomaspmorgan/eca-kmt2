using ECA.Business.Queries.Persons;
using System.Data.Entity;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using FluentValidation.Results;
using ECA.Business.Validation.Model;
using Newtonsoft.Json;
using ECA.Core.Exceptions;
using FluentValidation;

namespace ECA.Business.Service.Persons
{
    public class ExchangeVisitorValidationService : EcaService
    {
        private readonly Action<int, object, Type> throwIfModelDoesNotExist;
        private readonly Action<int, int, Participant> throwSecurityViolationIfParticipantDoesNotBelongToProject;
        private IExchangeVisitorService exchangeVisitorService;


        public ExchangeVisitorValidationService(EcaContext context,
            IExchangeVisitorService exchangeVisitorService,
            AbstractValidator<UpdateExchVisitor> updateExchVisitorValidator = null,
            AbstractValidator<CreateExchVisitor> createExchVisitorValidator = null,
            List<ISaveAction> saveActions = null)
            : base(context)
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

        public ParticipantPersonSevisCommStatus UpdateParticipantSevisValidation(User user, int projectId, int participantId)
        {
            var participant = Context.Participants.Find(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);

            var participantPerson = Context.ParticipantPersons.Find(participantId);
            throwIfModelDoesNotExist(participantId, participantPerson, typeof(ParticipantPerson));

            var validatableParticipant = ExchangeVisitorQueries.CreateGetValidatableParticipantsByParticipantIdsQuery(this.Context, new List<int> { participantId }).FirstOrDefault();
            if (validatableParticipant != null)
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
                return HandleNonValidatableParticipant(participantPerson);
            }
        }

        public async Task<ParticipantPersonSevisCommStatus> UpdateParticipantSevisValidationAsync(User user, int projectId, int participantId)
        {
            var participant = await Context.Participants.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);

            var participantPerson = await Context.ParticipantPersons.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participantPerson, typeof(ParticipantPerson));

            var validatableParticipant = await ExchangeVisitorQueries.CreateGetValidatableParticipantsByParticipantIdsQuery(this.Context, new List<int> { participantId }).FirstOrDefaultAsync();
            if (validatableParticipant != null)
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
                return HandleValidationResult(participantPerson, validationResult);
            }
            else
            {
                return HandleNonValidatableParticipant(participantPerson);
            }
        }

        private ParticipantPersonSevisCommStatus HandleValidationResult(ParticipantPerson person, ValidationResult result)
        {
            if (result.IsValid)
            {
                person.SevisValidationResult = null;
                return AddParticipantPersonSevisCommStatus(person.ParticipantId, SevisCommStatus.ReadyToSubmit.Id);
            }
            else
            {
                person.SevisValidationResult = JsonConvert.SerializeObject(result);
                return AddParticipantPersonSevisCommStatus(person.ParticipantId, SevisCommStatus.InformationRequired.Id);
            }
        }

        private ParticipantPersonSevisCommStatus HandleNonValidatableParticipant(ParticipantPerson person)
        {
            return AddParticipantPersonSevisCommStatus(person.ParticipantId, SevisCommStatus.InformationRequired.Id);
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


    }
}
