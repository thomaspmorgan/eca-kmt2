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
using System.Threading.Tasks;
using NLog;
using ECA.Core.Exceptions;

namespace ECA.Business.Service.Persons
{

    /// <summary>
    /// A ParticipantExchangeVisitorService is capable of performing crud operations on participant exchange visitors in the ECA system.
    /// </summary>
    public class ParticipantExchangeVisitorService : DbContextService<EcaContext>, IParticipantExchangeVisitorService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<int, object, Type> throwIfModelDoesNotExist;
        private Action<int, int, Participant> throwSecurityViolationIfParticipantDoesNotBelongToProject;

        /// <summary>
        /// Creates a new ParticipantExchangeVisitorService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ParticipantExchangeVisitorService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
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
                        String.Format("The user with id [{0}] attempted to delete a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        userId,
                        participant.ParticipantId,
                        projectId));
                }
            };
        }

        #region Get
        /// <summary>
        /// Returns a participantExchangeVisitors
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participantExchangeVisitors</returns>
        public ParticipantExchangeVisitorDTO GetParticipantExchangeVisitorById(int projectId, int participantId)
        {
            var participantExchangeVisitor = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorDTOByIdQuery(this.Context, projectId, participantId).FirstOrDefault();
            this.logger.Trace("Retrieved participantExchangeVisitors by id [{0}].", participantId);
            return participantExchangeVisitor;
        }

        /// <summary>
        /// Returns a participantExchangeVisitors asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantExchangeVisitors</returns>
        public Task<ParticipantExchangeVisitorDTO> GetParticipantExchangeVisitorByIdAsync(int projectId, int participantId)
        {
            var participantExchangeVisitor = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorDTOByIdQuery(this.Context, projectId, participantId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved participantExchangeVisitor by id [{0}].", participantId);
            return participantExchangeVisitor;
        }
        #endregion

        #region update

        /// <summary>
        /// Updates a participant person exchange visitor  info with given updated exchange visitor  information.
        /// </summary>
        /// <param name="updatedParticipantStudentVistor">The updated participant person exchange visitor  info.</param>
        public void Update(UpdatedParticipantExchangeVisitor updatedParticipantExchangeVisitor)
        {
            var participantExchangeVisitor = CreateGetParticipantExchangeVisitorByIdQuery(updatedParticipantExchangeVisitor.ParticipantId).FirstOrDefault();
            throwIfModelDoesNotExist(updatedParticipantExchangeVisitor.ParticipantId, participantExchangeVisitor, typeof(ParticipantExchangeVisitor));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(updatedParticipantExchangeVisitor.Audit.User.Id, updatedParticipantExchangeVisitor.ProjectId, participantExchangeVisitor.Participant);
            DoUpdate(participantExchangeVisitor, updatedParticipantExchangeVisitor);
        }

        /// <summary>
        /// Updates a participant person student visitor info with given updated exchange visitor  information.
        /// </summary>
        /// <param name="updatedParticipantExchangeVisitor">The updated participant person exchange visitor  info.</param>
        /// <returns>The task.</returns>
        public async Task UpdateAsync(UpdatedParticipantExchangeVisitor updatedParticipantExchangeVisitor)
        {
            var participantExchangeVisitor = await CreateGetParticipantExchangeVisitorByIdQuery(updatedParticipantExchangeVisitor.ParticipantId).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(updatedParticipantExchangeVisitor.ParticipantId, participantExchangeVisitor, typeof(ParticipantExchangeVisitor));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(updatedParticipantExchangeVisitor.Audit.User.Id, updatedParticipantExchangeVisitor.ProjectId, participantExchangeVisitor.Participant);
            DoUpdate(participantExchangeVisitor, updatedParticipantExchangeVisitor);
        }

        public async Task CreateParticipantExchangeVisitor(int participantId, User creator)
        {
            var participant = await Context.Participants.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));

            var participantExchangeVisitor = await Context.ParticipantExchangeVisitors.FindAsync(participantId);
            if (participantExchangeVisitor == null)
            {
                DoCreateParticipantExchangeVistor(participantId, creator);
            }
        }

        private void DoUpdate(ParticipantExchangeVisitor participantExchangeVisitor, UpdatedParticipantExchangeVisitor updatedParticipantExchangeVisitor)
        {
            //participantPersonValidator.ValidateUpdate(GetUpdatedPersonParticipantValidationEntity(participantType));
            updatedParticipantExchangeVisitor.Audit.SetHistory(participantExchangeVisitor);

            participantExchangeVisitor.FieldOfStudyId = updatedParticipantExchangeVisitor.FieldOfStudyId;
            participantExchangeVisitor.PositionId = updatedParticipantExchangeVisitor.PositionId;
            participantExchangeVisitor.ProgramCategoryId = updatedParticipantExchangeVisitor.ProgramCategoryId;
            participantExchangeVisitor.FundingSponsor = updatedParticipantExchangeVisitor.FundingSponsor;
            participantExchangeVisitor.FundingPersonal = updatedParticipantExchangeVisitor.FundingPersonal;
            participantExchangeVisitor.FundingVisGovt = updatedParticipantExchangeVisitor.FundingVisGovt;
            participantExchangeVisitor.FundingVisBNC = updatedParticipantExchangeVisitor.FundingVisBNC;
            participantExchangeVisitor.FundingGovtAgency1 = updatedParticipantExchangeVisitor.FundingGovtAgency1;
            participantExchangeVisitor.GovtAgency1Id = updatedParticipantExchangeVisitor.GovtAgency1Id;
            participantExchangeVisitor.GovtAgency1OtherName = updatedParticipantExchangeVisitor.GovtAgency1OtherName;
            participantExchangeVisitor.FundingGovtAgency2 = updatedParticipantExchangeVisitor.FundingGovtAgency2;
            participantExchangeVisitor.GovtAgency2Id = updatedParticipantExchangeVisitor.GovtAgency2Id;
            participantExchangeVisitor.GovtAgency2OtherName = updatedParticipantExchangeVisitor.GovtAgency2OtherName;
            participantExchangeVisitor.FundingIntlOrg1 = updatedParticipantExchangeVisitor.FundingIntlOrg1;
            participantExchangeVisitor.IntlOrg1Id = updatedParticipantExchangeVisitor.IntlOrg1Id;
            participantExchangeVisitor.IntlOrg1OtherName = updatedParticipantExchangeVisitor.IntlOrg1OtherName;
            participantExchangeVisitor.FundingIntlOrg2 = updatedParticipantExchangeVisitor.FundingIntlOrg2;
            participantExchangeVisitor.IntlOrg2Id = updatedParticipantExchangeVisitor.IntlOrg2Id;
            participantExchangeVisitor.IntlOrg2OtherName = updatedParticipantExchangeVisitor.IntlOrg2OtherName;
            participantExchangeVisitor.FundingOther = updatedParticipantExchangeVisitor.FundingOther;
            participantExchangeVisitor.OtherName = updatedParticipantExchangeVisitor.OtherName;
            participantExchangeVisitor.FundingTotal = updatedParticipantExchangeVisitor.FundingTotal;
        }

        private void DoCreateParticipantExchangeVistor(int participantId, User creator)
        {
            var newParticipantExchangeVisitor = new NewParticipantExchangeVisitor(creator,participantId);
            var participantExchangeVisitor = new ParticipantExchangeVisitor();
            participantExchangeVisitor.ParticipantId = participantId;
            newParticipantExchangeVisitor.Audit.SetHistory(participantExchangeVisitor);
            Context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
        }

        private IQueryable<ParticipantExchangeVisitor> CreateGetParticipantExchangeVisitorByIdQuery(int participantId)
        {
            return Context.ParticipantExchangeVisitors.Include(x => x.Participant).Where(x => x.ParticipantId == participantId);
        }

        #endregion
    }
}
