using ECA.Business.Queries.Persons;
using ECA.Business.Validation.Model;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public class PersonSevisServiceValidator : DbContextService<EcaContext>, IPersonSevisServiceValidator
    {
        private Action<int, int, Participant> throwSecurityViolationIfParticipantDoesNotBelongToProject;
        private readonly Action<int, object, Type> throwIfEntityNotFound;
        
        public PersonSevisServiceValidator(EcaContext context) : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
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
            throwIfEntityNotFound = (id, instance, t) =>
            {
                if (instance == null)
                {
                    throw new ModelNotFoundException(String.Format("The model type [{0}] with Id [{1}] was not found.", t.Name, id));
                }
            };
        }

        /// <summary>
        /// Do validation for sevis exchange visitor create
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>        
        public FluentValidation.Results.ValidationResult ValidateSevisCreateEV(int projectId, int participantId, User user)
        {
            var participant = this.Context.Participants.Find(participantId);
            throwIfEntityNotFound(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);
            var createEV = ParticipantPersonsSevisQueries.GetCreateExchangeVisitor(participantId, user, this.Context);

            var validator = new CreateExchVisitorValidator();
            var results = validator.Validate(createEV);
            
            return results;
        }
        
        /// <summary>
        /// Do validation for sevis exchange visitor create
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        public async Task<FluentValidation.Results.ValidationResult> ValidateSevisCreateEVAsync(int projectId, int participantId, User user)
        {
            var participant = await this.Context.Participants.FindAsync(participantId);
            throwIfEntityNotFound(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);
            var createEV = ParticipantPersonsSevisQueries.GetCreateExchangeVisitor(participantId, user, this.Context);

            var validator = new CreateExchVisitorValidator();
            var results = await validator.ValidateAsync(createEV);

            foreach (var error in results.Errors)
            {
                //Use a call to WithState to associate any piece of information with a ValidationFailure
                var test = error.CustomState;
            }

            return results;
        }
        
        /// <summary>
        /// Do validation for sevis exchange visitor update
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>        
        public FluentValidation.Results.ValidationResult ValidateSevisUpdateEV(int projectId, int participantId, User user)
        {
            var participant = this.Context.Participants.Find(participantId);
            throwIfEntityNotFound(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);
            var updateEV = ParticipantPersonsSevisQueries.GetUpdateExchangeVisitor(participantId, user, this.Context);

            var validator = new UpdateExchVisitorValidator();
            var results = validator.Validate(updateEV);
            
            return results;
        }

        /// <summary>
        /// Do validation for sevis exchange visitor update
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>Sevis object validation results</returns>
        public async Task<FluentValidation.Results.ValidationResult> ValidateSevisUpdateEVAsync(int projectId, int participantId, User user)
        {
            var participant = await this.Context.Participants.FindAsync(participantId);
            throwIfEntityNotFound(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);
            var updateEV = ParticipantPersonsSevisQueries.GetUpdateExchangeVisitor(participantId, user, this.Context);

            var validator = new UpdateExchVisitorValidator();
            var results = await validator.ValidateAsync(updateEV);
            
            return results;
        }
    }
}
