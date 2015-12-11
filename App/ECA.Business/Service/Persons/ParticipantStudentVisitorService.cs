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
    /// A ParticipantStudentVisitorService is capable of performing crud operations on participant student visitors in the ECA system.
    /// </summary>
    public class ParticipantStudentVisitorService : DbContextService<EcaContext>, IParticipantStudentVisitorService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<int, object, Type> throwIfModelDoesNotExist;

        /// <summary>
        /// Creates a new ParticipantStudentVisitorService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ParticipantStudentVisitorService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
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
        /// Returns the participantStudentVisitors in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantStudentVisitors.</returns>
        public PagedQueryResults<ParticipantStudentVisitorDTO> GetParticipantStudentVisitors(QueryableOperator<ParticipantStudentVisitorDTO> queryOperator)
        {
            var participantStudentVisitors = ParticipantStudentVisitorQueries.CreateGetParticipantStudentVisitorsDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantStudentVisitors with query operator [{0}].", queryOperator);
            return participantStudentVisitors;
        }

        /// <summary>
        /// Returns the participantStudentVisitors in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantStudentVisitors.</returns>
        public Task<PagedQueryResults<ParticipantStudentVisitorDTO>> GetParticipantStudentVisitorsAsync(QueryableOperator<ParticipantStudentVisitorDTO> queryOperator)
        {
            var participantStudentVisitors = ParticipantStudentVisitorQueries.CreateGetParticipantStudentVisitorsDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantStudentVisitors with query operator [{0}].", queryOperator);
            return participantStudentVisitors;
        }

        /// <summary>
        /// Returns the participantStudentVisitors for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantStudentVisitors.</returns>
        public PagedQueryResults<ParticipantStudentVisitorDTO> GetParticipantStudentVisitorsByProjectId(int projectId, QueryableOperator<ParticipantStudentVisitorDTO> queryOperator)
        {
            var participantStudentVisitors = ParticipantStudentVisitorQueries.CreateGetParticipantStudentVisitorsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantStudentVisitors by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantStudentVisitors;
        }

        /// <summary>
        /// Returns the participantStudentVisitors for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantStudentVisitors.</returns>
        public Task<PagedQueryResults<ParticipantStudentVisitorDTO>> GetParticipantStudentVisitorsByProjectIdAsync(int projectId, QueryableOperator<ParticipantStudentVisitorDTO> queryOperator)
        {
            var participantStudentVisitors = ParticipantStudentVisitorQueries.CreateGetParticipantStudentVisitorsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantStudentVisitors by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantStudentVisitors;
        }

        /// <summary>
        /// Returns a participantStudentVisitor
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participantStudentVisitor</returns>
        public ParticipantStudentVisitorDTO GetParticipantStudentVisitorById(int participantId)
        {
            var participantStudentVisitor = ParticipantStudentVisitorQueries.CreateGetParticipantStudentVisitorDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            this.logger.Trace("Retrieved participantStudentVisitor by id [{0}].", participantId);
            return participantStudentVisitor;
        }

        /// <summary>
        /// Returns a participantStudentVisitor asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantStudentVisitor</returns>
        public Task<ParticipantStudentVisitorDTO> GetParticipantStudentVisitorByIdAsync(int participantId)
        {
            var participantStudentVisitor = ParticipantStudentVisitorQueries.CreateGetParticipantStudentVisitorDTOByIdQuery(this.Context, participantId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved participantStudentVisitor by id [{0}].", participantId);
            return participantStudentVisitor;
        }
        #endregion

        #region update

        /// <summary>
        /// Updates a participant person student visitor  info with given updated student visitor  information.
        /// </summary>
        /// <param name="updatedParticipantStudentVistor">The updated participant person student visitor  info.</param>
        public ParticipantStudentVisitorDTO Update(UpdatedParticipantStudentVisitor updatedParticipantStudentVistor)
        {
            var participantStudentVisitor = CreateGetParticipantStudentVisitorByIdQuery(updatedParticipantStudentVistor.ParticipantId).FirstOrDefault();
            throwIfModelDoesNotExist(updatedParticipantStudentVistor.ParticipantId, participantStudentVisitor, typeof(ParticipantStudentVisitor));

            DoUpdate(participantStudentVisitor, updatedParticipantStudentVistor);
            return this.GetParticipantStudentVisitorById(updatedParticipantStudentVistor.ParticipantId);
        }

        /// <summary>
        /// Updates a participant person student visitor info with given updated student visitor  information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person student visitor  info.</param>
        /// <returns>The task.</returns>
        public async Task<ParticipantStudentVisitorDTO> UpdateAsync(UpdatedParticipantStudentVisitor updatedParticipantStudentVisitor)
        {
            var participantPerson = await CreateGetParticipantStudentVisitorByIdQuery(updatedParticipantStudentVisitor.ParticipantId).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(updatedParticipantStudentVisitor.ParticipantId, participantPerson, typeof(ParticipantStudentVisitor));

            DoUpdate(participantPerson, updatedParticipantStudentVisitor);

            return await this.GetParticipantStudentVisitorByIdAsync(updatedParticipantStudentVisitor.ParticipantId);
        }

        public async Task CreateParticipantStudentVisitor(int participantId, User creator)
        {
            var participant = await Context.Participants.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));

            var participantStudentVisitor = await Context.ParticipantStudentVisitors.FindAsync(participantId);
            if (participantStudentVisitor == null)
            {
                DoCreateParticipantStudentVistor(participantId, creator);
            }
        }

        private void DoUpdate(ParticipantStudentVisitor participantStudentVisitor, UpdatedParticipantStudentVisitor updatedParticipantStudentVisitor)
        {
            //participantPersonValidator.ValidateUpdate(GetUpdatedPersonParticipantValidationEntity(participantType));
            updatedParticipantStudentVisitor.Audit.SetHistory(participantStudentVisitor);

            participantStudentVisitor.EducationLevelId = updatedParticipantStudentVisitor.EducationLevelId;
            participantStudentVisitor.EducationLevelOtherRemarks = updatedParticipantStudentVisitor.EducationLevelOtherRemarks;
            participantStudentVisitor.PrimaryMajorId = updatedParticipantStudentVisitor.PrimaryMajorId;
            participantStudentVisitor.SecondaryMajorId = updatedParticipantStudentVisitor.SecondaryMajorId;
            participantStudentVisitor.MinorId = updatedParticipantStudentVisitor.MinorId;
            participantStudentVisitor.LengthOfStudy = updatedParticipantStudentVisitor.LengthOfStudy;
            participantStudentVisitor.StudyProject = updatedParticipantStudentVisitor.StudyProject;
            participantStudentVisitor.IsEnglishProficiencyReqd = updatedParticipantStudentVisitor.IsEnglishLanguageProficiencyReqd;
            participantStudentVisitor.IsEnglishProficiencyMet = updatedParticipantStudentVisitor.IsEnglishLanguageProficiencyMet;
            participantStudentVisitor.EnglishProficiencyNotReqdReason = updatedParticipantStudentVisitor.EnglishLanguageProficiencyNotReqdReason;
            participantStudentVisitor.TuitionExpense = updatedParticipantStudentVisitor.TuitionExpense;
            participantStudentVisitor.LivingExpense = updatedParticipantStudentVisitor.LivingExpense;
            participantStudentVisitor.DependentExpense = updatedParticipantStudentVisitor.DependentExpense;
            participantStudentVisitor.OtherExpense = updatedParticipantStudentVisitor.OtherExpense;
            participantStudentVisitor.ExpenseRemarks = updatedParticipantStudentVisitor.ExpenseRemarks;
            participantStudentVisitor.PersonalFunding = updatedParticipantStudentVisitor.PersonalFunding;
            participantStudentVisitor.SchoolFunding = updatedParticipantStudentVisitor.SchoolFunding;
            participantStudentVisitor.SchoolFundingRemarks = updatedParticipantStudentVisitor.SchoolFundingRemarks;
            participantStudentVisitor.OtherFunding = updatedParticipantStudentVisitor.OtherFunding;
            participantStudentVisitor.OtherFundingRemarks = updatedParticipantStudentVisitor.OtherFundingRemarks;
            participantStudentVisitor.EmploymentFunding = updatedParticipantStudentVisitor.EmploymentFunding;
        }

        private void DoCreateParticipantStudentVistor(int participantId, User creator)
        {
            var newParticipantStudentVisitor = new NewParticipantStudentVisitor(creator,participantId);
            var participantStudentVisitor = new ParticipantStudentVisitor();
            participantStudentVisitor.ParticipantId = participantId;
            newParticipantStudentVisitor.Audit.SetHistory(participantStudentVisitor);
            Context.ParticipantStudentVisitors.Add(participantStudentVisitor);
        }

        private IQueryable<ParticipantStudentVisitor> CreateGetParticipantStudentVisitorByIdQuery(int participantId)
        {
            return Context.ParticipantStudentVisitors.Where(x => x.ParticipantId == participantId);
        }

        #endregion
    }
}
