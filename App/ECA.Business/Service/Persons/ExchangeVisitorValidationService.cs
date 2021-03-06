﻿using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using ECA.Business.Queries.Persons;
using ECA.Business.Validation;
using ECA.Business.Validation.Sevis;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
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
        private IExchangeVisitorService exchangeVisitorService;
        private IParticipantPersonsSevisService participantPersonSevisService;

        /// <summary>
        /// Creates a new instance of the service with the context to query and the validators.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="exchangeVisitorService">The exchange visitor service that is capable of executing validation.</param>
        /// <param name="exchangeVisitorValidator">The update exchange visitor validator.</param>
        /// <param name="saveActions">The context save actions.</param>
        public ExchangeVisitorValidationService(
            EcaContext context,
            IExchangeVisitorService exchangeVisitorService,
            IParticipantPersonsSevisService participantPersonSevisService,
            AbstractValidator<ExchangeVisitor> exchangeVisitorValidator = null,
            List<ISaveAction> saveActions = null)
            : base(context, saveActions)
        {
            Contract.Requires(exchangeVisitorService != null, "The exchange visitor service must not be null.");
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(participantPersonSevisService != null, "The participantPersonSevisService must not be null.");
            this.exchangeVisitorService = exchangeVisitorService;
            this.participantPersonSevisService = participantPersonSevisService;
            if (exchangeVisitorValidator == null)
            {
                this.ExchangeVisitorValidator = new ExchangeVisitorValidator();
            }
            else
            {
                this.ExchangeVisitorValidator = exchangeVisitorValidator;
            }
            throwIfModelDoesNotExist = (id, instance, type) =>
            {
                if (instance == null)
                {
                    throw new ModelNotFoundException(String.Format("The model of type [{0}] with id [{1}] was not found.", type.Name, id));
                }
            };
        }

        /// <summary>
        /// Gets the exchange visitor validator.
        /// </summary>
        public AbstractValidator<ExchangeVisitor> ExchangeVisitorValidator { get; private set; }

        /// <summary>
        /// Runs a validation on sevis information for the participant with the given id and updates the sevis comm
        /// status with the latest validation result.  If the sevis validation is successful then a sevis comm status is added
        /// with the Ready To Submit status, otherwise, Information Required is set.
        /// </summary>
        /// <param name="user">The user performing the validation.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The id of the participant.</param>
        /// <returns>The added participant sevis comm status.</returns>
        public ParticipantPersonSevisCommStatus RunParticipantSevisValidation(int projectId, int participantId)
        {
            var participant = Context.Participants.Find(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));

            var participantPerson = Context.ParticipantPersons.Find(participantId);
            throwIfModelDoesNotExist(participantId, participantPerson, typeof(ParticipantPerson));

            if (ShouldRunValidation(participant, participantPerson))
            {
                var exchangeVisitor = this.exchangeVisitorService.GetExchangeVisitor(projectId, participantId);
                var hasChanges = String.IsNullOrWhiteSpace(exchangeVisitor.SevisId);
                if (!hasChanges)
                {
                    var history = Context.ExchangeVisitorHistories.Find(participantId);
                    if (history != null && history.LastSuccessfulModel != null)
                    {
                        var previouslySubmittedExchangeVisitor = ExchangeVisitor.GetExchangeVisitor(history.LastSuccessfulModel);
                        //either the previous exchange visitor has differences with the exchange visitor, or the previous exchange visitor
                        //does not have a sevis id i.e. it was just created and now we have a sevis id
                        hasChanges = exchangeVisitor.HasChanges(previouslySubmittedExchangeVisitor) || String.IsNullOrWhiteSpace(previouslySubmittedExchangeVisitor.SevisId);
                    }
                    else
                    {
                        hasChanges = true;
                    }
                }
                if (hasChanges)
                {
                    ValidationResult validationResult = exchangeVisitor.Validate(this.ExchangeVisitorValidator);
                    return HandleValidationResult(exchangeVisitor, participantPerson, validationResult);
                }
                else
                {
                    HandleNonValidatedParticipant(participantPerson);
                    return null;
                }
            }
            else
            {
                HandleNonValidatedParticipant(participantPerson);
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
        public async Task<ParticipantPersonSevisCommStatus> RunParticipantSevisValidationAsync(int projectId, int participantId)
        {
            var participant = await Context.Participants.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));

            var participantPerson = await Context.ParticipantPersons.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participantPerson, typeof(ParticipantPerson));

            if (ShouldRunValidation(participant, participantPerson))
            {
                var exchangeVisitor = await this.exchangeVisitorService.GetExchangeVisitorAsync(projectId, participantId);
                var hasChanges = String.IsNullOrWhiteSpace(exchangeVisitor.SevisId);
                if (!hasChanges)
                {
                    var history = await Context.ExchangeVisitorHistories.FindAsync(participantId);
                    if (history != null && history.LastSuccessfulModel != null)
                    {
                        var previouslySubmittedExchangeVisitor = ExchangeVisitor.GetExchangeVisitor(history.LastSuccessfulModel);
                        //either the previous exchange visitor has differences with the exchange visitor, or the previous exchange visitor
                        //does not have a sevis id i.e. it was just created and now we have a sevis id
                        hasChanges = exchangeVisitor.HasChanges(previouslySubmittedExchangeVisitor) || String.IsNullOrWhiteSpace(previouslySubmittedExchangeVisitor.SevisId);
                    }
                    else
                    {
                        hasChanges = true;
                    }
                }
                if (hasChanges)
                {
                    ValidationResult validationResult = exchangeVisitor.Validate(this.ExchangeVisitorValidator);
                    return await HandleValidationResultAsync(exchangeVisitor, participantPerson, validationResult);
                }
                else
                {
                    HandleNonValidatedParticipant(participantPerson);
                    return null;
                }
            }
            else
            {
                HandleNonValidatedParticipant(participantPerson);
                return null;
            }
        }

        private void HandleNonValidatedParticipant(ParticipantPerson person)
        {
            person.SevisValidationResult = null;
        }

        /// <summary>
        /// Returns true if all conditions are met stating sevis validation should run on a participant.
        /// </summary>
        /// <param name="participant">The participant.</param>
        /// <param name="participantPerson">The participant person.</param>
        /// <returns>True, if sevis exchange visitor validation should run; otherwise, false.</returns>
        public bool ShouldRunValidation(Participant participant, ParticipantPerson participantPerson)
        {
            if (participant.ParticipantTypeId != ParticipantType.ForeignTravelingParticipant.Id)
            {
                return false;
            }
            if (!participant.ParticipantStatusId.HasValue)
            {
                return false;
            }
            if (!ParticipantStatus.EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES.Select(x => x.Id).Contains(participant.ParticipantStatusId.Value))
            {
                return false;
            }
            if (participantPerson.IsCancelled || participantPerson.IsSentToSevisViaRTI || participantPerson.IsValidatedViaRTI)
            {
                return false;
            }
            return true;
        }

        #region Handle Validation Result

        private async Task<ParticipantPersonSevisCommStatus> HandleValidationResultAsync(ExchangeVisitor exchangeVisitor, ParticipantPerson person, ValidationResult result)
        {
            //remember this method should be as performant as possible since lots of different entity edits can cause this method to run
            var latestCommStatus = await CreateGetLatestParticipantPersonSevisCommStatusQuery(person.ParticipantId).FirstOrDefaultAsync();
            person.SevisValidationResult = GetSevisValidationResultAsJson(result);
            if (!result.IsValid)
            {
                if (!String.IsNullOrWhiteSpace(person.SevisId))
                {
                    var isParticipantReadyToValidate = await this.participantPersonSevisService.IsParticipantReadyToValidateAsync(person.ParticipantId);
                    if (isParticipantReadyToValidate)
                    {
                        return AddOrUpdateParticipantPersonSevisCommStatus(latestCommStatus, person.ParticipantId, SevisCommStatus.NeedsValidationInfo.Id);
                    }

                    var hasParticipantNeededValidationInfo = await CreateGetParticipantPersonSevisCommStatusBySevisCommStatusIdQuery(person.ParticipantId, SevisCommStatus.NeedsValidationInfo.Id).CountAsync() > 0;
                    if (!exchangeVisitor.IsValidated && hasParticipantNeededValidationInfo)
                    {
                        return AddOrUpdateParticipantPersonSevisCommStatus(latestCommStatus, person.ParticipantId, SevisCommStatus.NeedsValidationInfo.Id);
                    }

                    return AddOrUpdateParticipantPersonSevisCommStatus(latestCommStatus, person.ParticipantId, SevisCommStatus.InformationRequired.Id);
                }
                else
                {
                    return AddOrUpdateParticipantPersonSevisCommStatus(latestCommStatus, person.ParticipantId, SevisCommStatus.InformationRequired.Id);
                }
            }
            else
            {
                var hasParticipantNeededValidationInfo = await CreateGetParticipantPersonSevisCommStatusBySevisCommStatusIdQuery(person.ParticipantId, SevisCommStatus.NeedsValidationInfo.Id).CountAsync() > 0;
                var isParticipantReadyToValidate = await this.participantPersonSevisService.IsParticipantReadyToValidateAsync(person.ParticipantId);
                if ((hasParticipantNeededValidationInfo && !exchangeVisitor.IsValidated) || isParticipantReadyToValidate)
                {
                    return AddParticipantPersonSevisCommStatus(person.ParticipantId, SevisCommStatus.ReadyToValidate.Id);
                }
                else
                {
                    return AddParticipantPersonSevisCommStatus(person.ParticipantId, SevisCommStatus.ReadyToSubmit.Id);
                }
            }
        }

        private ParticipantPersonSevisCommStatus HandleValidationResult(ExchangeVisitor exchangeVisitor, ParticipantPerson person, ValidationResult result)
        {
            //remember this method should be as performant as possible since lots of different entity edits can cause this method to run
            person.SevisValidationResult = GetSevisValidationResultAsJson(result);
            var latestCommStatus = CreateGetLatestParticipantPersonSevisCommStatusQuery(person.ParticipantId).FirstOrDefault();
            if (!result.IsValid)
            {   
                if (!String.IsNullOrWhiteSpace(person.SevisId))
                {
                    var isParticipantReadyToValidate = this.participantPersonSevisService.IsParticipantReadyToValidate(person.ParticipantId);
                    if (isParticipantReadyToValidate)
                    {
                        return AddOrUpdateParticipantPersonSevisCommStatus(latestCommStatus, person.ParticipantId, SevisCommStatus.NeedsValidationInfo.Id);
                    }

                    var hasParticipantNeededValidationInfo = CreateGetParticipantPersonSevisCommStatusBySevisCommStatusIdQuery(person.ParticipantId, SevisCommStatus.NeedsValidationInfo.Id).Count() > 0;
                    if (!exchangeVisitor.IsValidated && hasParticipantNeededValidationInfo)
                    {
                        return AddOrUpdateParticipantPersonSevisCommStatus(latestCommStatus, person.ParticipantId, SevisCommStatus.NeedsValidationInfo.Id);
                    }

                    return AddOrUpdateParticipantPersonSevisCommStatus(latestCommStatus, person.ParticipantId, SevisCommStatus.InformationRequired.Id);
                }
                else
                {
                    return AddOrUpdateParticipantPersonSevisCommStatus(latestCommStatus, person.ParticipantId, SevisCommStatus.InformationRequired.Id);
                }
            }
            else
            {
                var hasParticipantNeededValidationInfo = CreateGetParticipantPersonSevisCommStatusBySevisCommStatusIdQuery(person.ParticipantId, SevisCommStatus.NeedsValidationInfo.Id).Count() > 0;
                var isParticipantReadyToValidate = this.participantPersonSevisService.IsParticipantReadyToValidate(person.ParticipantId);
                if ((hasParticipantNeededValidationInfo && !exchangeVisitor.IsValidated) || isParticipantReadyToValidate)
                {
                    return AddParticipantPersonSevisCommStatus(person.ParticipantId, SevisCommStatus.ReadyToValidate.Id);
                }
                else
                {
                    return AddParticipantPersonSevisCommStatus(person.ParticipantId, SevisCommStatus.ReadyToSubmit.Id);
                }
            }
        }

        private string GetSevisValidationResultAsJson(ValidationResult result)
        {
            SimpleValidationResult simpleValidationResult = null;
            if (result.IsValid)
            {
                simpleValidationResult = new SuccessfulValidationResult();
            }
            else
            {
                simpleValidationResult = new SimpleValidationResult(result);
            }
            return JsonConvert.SerializeObject(
                simpleValidationResult,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
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
                if (latestStatus.SevisCommStatusId == SevisCommStatus.InformationRequired.Id
                    || latestStatus.SevisCommStatusId == SevisCommStatus.NeedsValidationInfo.Id)
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

        private IQueryable<ParticipantPersonSevisCommStatus> CreateGetParticipantPersonSevisCommStatusBySevisCommStatusIdQuery(int participantId, int sevisCommStatusId)
        {
            return CreateGetParticipantPersonSevisCommStatusBySevisCommStatusIdQuery(participantId, new int[] { sevisCommStatusId });
        }

        private IQueryable<ParticipantPersonSevisCommStatus> CreateGetParticipantPersonSevisCommStatusBySevisCommStatusIdQuery(int participantId, IEnumerable<int> sevisCommStatusIds)
        {
            return Context.ParticipantPersonSevisCommStatuses.Where(x => x.ParticipantId == participantId && sevisCommStatusIds.ToList().Contains(x.SevisCommStatusId));
        }

        /// <summary>
        /// Returns the ExchangeVisitor validator object.
        /// </summary>
        /// <returns>The ExchangeVisitor validator.</returns>
        public AbstractValidator<ExchangeVisitor> GetValidator()
        {
            return this.ExchangeVisitorValidator;
        }
        #endregion
    }
}
