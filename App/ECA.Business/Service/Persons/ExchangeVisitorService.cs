using ECA.Business.Queries.Persons;
using System.Data.Entity;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Core.Exceptions;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public class ExchangeVisitorService : EcaService
    {
        /// <summary>
        /// The default exchange visitor occupation category code.
        /// </summary>
        public const string EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE = "99";

        private readonly Action<int, object, Type> throwIfModelDoesNotExist;
        private readonly Action<int, int, Participant> throwSecurityViolationIfParticipantDoesNotBelongToProject;
        private readonly Action<Participant> throwIfParticipantIsNotAPerson;
        private readonly Action<Participant, int> throwIfMoreThanOneCountryOfCitizenship;

        public ExchangeVisitorService(EcaContext context, List<ISaveAction> saveActions = null)
            : base(context, saveActions)
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
            throwIfParticipantIsNotAPerson = (participant) =>
            {
                if (!participant.PersonId.HasValue)
                {
                    throw new NotSupportedException(String.Format("The participant with id [0] is not a person participant.", participant.ParticipantId));
                }
            };
            throwIfMoreThanOneCountryOfCitizenship = (participant, numberOfCitizenships) =>
            {
                if (numberOfCitizenships > 1)
                {
                    throw new NotSupportedException(String.Format("The participant with id [0] has more than one country of citizenship.", participant.ParticipantId));
                }
            };
        }

        #region Get Create Exchange Visitor
        public CreateExchVisitor GetCreateExchangeVisitor(User user, int personId)
        {
            //do the lookup here for the person's participant info based on latest participant.
            throw new NotImplementedException();
        }

        public Task<CreateExchVisitor> GetCreateExchangeVisitorAsync(User user, int personId)
        {
            //do the lookup here for the person's participant info based on latest participant.
            throw new NotImplementedException();
        }

        public CreateExchVisitor GetCreateExchangeVisitor(User user, int projectId, int participantId)
        {
            throw new NotImplementedException();
        }

        public async Task<CreateExchVisitor> GetCreateExchangeVisitorAsync(User user, int projectId, int participantId)
        {
            var participant = await Context.Participants.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(user.Id, projectId, participant);
            throwIfParticipantIsNotAPerson(participant);

            var participantPerson = await Context.ParticipantPersons.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participantPerson, typeof(ParticipantPerson));

            var participantExchangeVisitor = await CreateGetParticipantExchangeVisitorByParticipantIdQuery(participantId).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(participantId, participantExchangeVisitor, typeof(ParticipantExchangeVisitor));

            //need to check for multiple countries of citizen...
            var numberOfCitizenships = await CreateGetNumberOfCitizenshipsQuery(participantId).CountAsync();
            throwIfMoreThanOneCountryOfCitizenship(participant, numberOfCitizenships);

            var project = await Context.Projects.FindAsync(participant.ProjectId);

            var exchangeVisitor = GetCreateExchangeVisitor(
                participant: participant,
                user: user, 
                project: project,
                visitor: participantExchangeVisitor);
            




            return new CreateExchVisitor
            {
                ExchangeVisitor = exchangeVisitor
            };

        }

        public ExchangeVisitor GetCreateExchangeVisitor(Participant participant, User user, Project project, ParticipantExchangeVisitor visitor)
        {
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(user != null, "The user must not be null.");
            Contract.Requires(project != null, "The project must not be null.");
            Contract.Requires(visitor != null, "The visitor must not be null.");
            var instance = new ExchangeVisitor();
            instance.requestID = participant.ParticipantId.ToString();
            instance.userID = user.Id.ToString();
            instance.PrgStartDate = project.StartDate.UtcDateTime;
            instance.PrgEndDate = project.EndDate.HasValue ? project.EndDate.Value.UtcDateTime : default(DateTime?);
            instance.OccupationCategoryCode = EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            if (visitor.Position != null)
            {
                instance.PositionCode = visitor.Position.PositionCode;
            }
            if (visitor.ProgramCategory != null)
            {
                instance.CategoryCode = visitor.ProgramCategory.ProgramCategoryCode;
            }
            return instance;
        }
        #endregion

        #region Get Update Exchange Visitor

        #endregion

        private IQueryable<ParticipantExchangeVisitor> CreateGetParticipantExchangeVisitorByParticipantIdQuery(int participantId)
        {
            return Context.ParticipantExchangeVisitors
                .Include(x => x.Position)
                .Include(x => x.ProgramCategory)
                .Where(x => x.ParticipantId == participantId);
        }

        private IQueryable<int> CreateGetNumberOfCitizenshipsQuery(int participantId)
        {
            return Context.Participants.Where(p => p.ParticipantId == participantId)
                .Select(x => x.Person)
                .SelectMany(x => x.CountriesOfCitizenship)
                .Select(x => x.LocationId);
        }
    }
}
