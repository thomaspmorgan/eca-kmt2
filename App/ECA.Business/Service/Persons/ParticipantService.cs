using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NLog;
using ECA.Core.Exceptions;

namespace ECA.Business.Service.Persons
{

    /// <summary>
    /// A ParticipantService is capable of performing crud operations on participants in the ECA system.
    /// </summary>
    public class ParticipantService : DbContextService<EcaContext>, ECA.Business.Service.Persons.IParticipantService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private Action<int, int, Participant> throwSecurityViolationIfParticipantDoesNotBelongToProject;
        private readonly Action<int, object, Type> throwIfEntityNotFound;

        /// <summary>
        /// Creates a new ParticipantService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ParticipantService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwSecurityViolationIfParticipantDoesNotBelongToProject = (userId, projectId, participant) =>
            {
                if (participant != null && participant.ProjectId != projectId)
                {
                    throw new BusinessSecurityException(
                        String.Format("The user with id [{0}] attempted to delete a participant with id [{1}] and project id [{2}] but should have been denied access.",
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

        #region Get

        /// <summary>
        /// Returns the participants in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants.</returns>
        public PagedQueryResults<SimpleParticipantDTO> GetParticipants(QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            var participants = ParticipantQueries.CreateGetSimpleParticipantsDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participants with query operator [{0}].", queryOperator);
            return participants;
        }

        /// <summary>
        /// Returns the participants in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants.</returns>
        public Task<PagedQueryResults<SimpleParticipantDTO>> GetParticipantsAsync(QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            var participants = ParticipantQueries.CreateGetSimpleParticipantsDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participants with query operator [{0}].", queryOperator);
            return participants;
        }

        /// <summary>
        /// Returns the participants for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participants.</returns>
        public PagedQueryResults<SimpleParticipantDTO> GetParticipantsByProjectId(int projectId, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            var participants = ParticipantQueries.CreateGetSimpleParticipantsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participants by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participants;
        }

        /// <summary>
        /// Returns the participants for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participants.</returns>
        public Task<PagedQueryResults<SimpleParticipantDTO>> GetParticipantsByProjectIdAsync(int projectId, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            var participants = ParticipantQueries.CreateGetSimpleParticipantsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participants by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participants;
        }

        /// <summary>
        /// Returns a participant 
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participant</returns>
        public ParticipantDTO GetParticipantById(int participantId)
        {
            var participant = ParticipantQueries.CreateGetParticipantDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            this.logger.Trace("Retrieved participant by id [{0}].", participantId);
            return participant;
        }

        /// <summary>
        /// Returns a participant asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participant</returns>
        public Task<ParticipantDTO> GetParticipantByIdAsync(int participantId)
        {
            var participant = ParticipantQueries.CreateGetParticipantDTOByIdQuery(this.Context, participantId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved participant by id [{0}].", participantId);
            return participant;
        }
        #endregion

        #region Delete

        /// <summary>
        /// Deletes the participant from the datastore given the DeletedParticipant business entity.
        /// </summary>
        /// <param name="deletedParticipant">The business entity.</param>
        public void Delete(DeletedParticipant deletedParticipant)
        {
            var participant = Context.Participants.Find(deletedParticipant.ParticipantId);
            throwIfEntityNotFound(deletedParticipant.ParticipantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(deletedParticipant.Audit.User.Id, deletedParticipant.ProjectId, participant);

            var project = Context.Projects.Find(deletedParticipant.ProjectId);
            throwIfEntityNotFound(deletedParticipant.ProjectId, project, typeof(Project));

            var participantPerson = Context.ParticipantPersons.Find(deletedParticipant.ParticipantId);
            var studentVisitor = Context.ParticipantStudentVisitors.Find(deletedParticipant.ParticipantId);
            var statti = Context.ParticipantPersonSevisCommStatuses.Where(x => x.ParticipantId == deletedParticipant.ParticipantId).ToList();

            var sourceMoneyFlows = Context.MoneyFlows.Where(x => x.SourceParticipantId == deletedParticipant.ParticipantId).ToList();
            var recipientMoneyFlows = Context.MoneyFlows.Where(x => x.RecipientParticipantId == deletedParticipant.ParticipantId).ToList();
            DoDelete(deletedParticipant: deletedParticipant,
                project: project,
                participant: participant,
                person: participantPerson,
                studentVisitor: studentVisitor,
                statii: statti,
                participantRecipientMoneyFlows: recipientMoneyFlows,
                participantSourceMoneyFlows: sourceMoneyFlows);
        }

        /// <summary>
        /// Deletes the participant from the datastore given the DeletedParticipant business entity.
        /// </summary>
        /// <param name="deletedParticipant">The business entity.</param>
        public async Task DeleteAsync(DeletedParticipant deletedParticipant)
        {
            var participant = await Context.Participants.FindAsync(deletedParticipant.ParticipantId);
            throwIfEntityNotFound(deletedParticipant.ParticipantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(deletedParticipant.Audit.User.Id, deletedParticipant.ProjectId, participant);

            var project = await Context.Projects.FindAsync(deletedParticipant.ProjectId);
            throwIfEntityNotFound(deletedParticipant.ProjectId, project, typeof(Project));

            var participantPerson = await Context.ParticipantPersons.FindAsync(deletedParticipant.ParticipantId);
            var studentVisitor = await Context.ParticipantStudentVisitors.FindAsync(deletedParticipant.ParticipantId);

            var statti = await Context.ParticipantPersonSevisCommStatuses.Where(x => x.ParticipantId == deletedParticipant.ParticipantId).ToListAsync();

            var sourceMoneyFlows = await Context.MoneyFlows.Where(x => x.SourceParticipantId == deletedParticipant.ParticipantId).ToListAsync();
            var recipientMoneyFlows = await Context.MoneyFlows.Where(x => x.RecipientParticipantId == deletedParticipant.ParticipantId).ToListAsync();
            DoDelete(deletedParticipant: deletedParticipant,
                project: project,
                participant: participant,
                person: participantPerson,
                studentVisitor: studentVisitor,
                statii: statti,
                participantRecipientMoneyFlows: recipientMoneyFlows,
                participantSourceMoneyFlows: sourceMoneyFlows);
        }

        private void DoDelete(
            DeletedParticipant deletedParticipant,
            Project project,
            Participant participant,
            ParticipantPerson person,
            ParticipantStudentVisitor studentVisitor,
            IEnumerable<ParticipantPersonSevisCommStatus> statii,
            IEnumerable<MoneyFlow> participantSourceMoneyFlows,
            IEnumerable<MoneyFlow> participantRecipientMoneyFlows)
        {
            Contract.Requires(participant != null, "The participant must not be null.");
            Contract.Requires(project != null, "The project must not be null.");
            Context.ParticipantPersonSevisCommStatuses.RemoveRange(statii);
            Context.MoneyFlows.RemoveRange(participantSourceMoneyFlows);
            Context.MoneyFlows.RemoveRange(participantRecipientMoneyFlows);
            if (studentVisitor != null)
            {
                Context.ParticipantStudentVisitors.Remove(studentVisitor);
            }
            if (person != null)
            {
                Context.ParticipantPersons.Remove(person);
            }
            Context.Participants.Remove(participant);
            deletedParticipant.Audit.SetHistory(project);
        }
        #endregion
    }
}
