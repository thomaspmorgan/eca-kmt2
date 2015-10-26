using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// The ParticipantStudentVisitorQueries are used to query a DbContext for Participant Student Visitor information.
    /// </summary>
    public static class ParticipantStudentVisitorQueries
    {
        /// <summary>
        /// Query to get a list of participant people with Student Visitor info
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <returns>List of participant student visitors with SEVIS</returns>
        public static IQueryable<ParticipantStudentVisitorDTO> CreateGetParticipantStudentVisitorsDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = (from p in context.ParticipantStudentVisitors
                         select new ParticipantStudentVisitorDTO
                         {
                             ParticipantId = p.ParticipantId,
                             ProjectId = p.Participant.ProjectId,
                             IssueReasonId = p.IssueReasonId,
                             IssueReason = p.IssueReason == null ? string.Empty : p.IssueReason.Description,
                             EducationLevelId = p.EducationLevelId,
                             EducationLevel = p.EducationLevel == null ? string.Empty : p.EducationLevel.Description,
                             EducationLevelOtherRemarks = p.EducationLevelOtherRemarks,
                             PrimaryMajorId = p.PrimaryMajorId,
                             PrimaryMajor = p.PrimaryMajor.Description,
                             SecondaryMajorId = p.SecondaryMajorId,
                             SecondaryMajor = p.SecondaryMajor == null ? string.Empty : p.SecondaryMajor.Description,
                             MinorId = p.MinorId,
                             Minor = p.Minor == null ? string.Empty : p.Minor.Description,
                             LengthOfStudy = p.LengthOfStudy,
                             IsEnglishLanguageProficiencyReqd = p.IsEnglishProficiencyReqd,
                             IsEnglishLanguageProficiencyMet = p.IsEnglishProficiencyMet,
                             EnglishLanguageProficiencyNotReqdReason = p.EnglishProficiencyNotReqdReason,
                             TuitionExpense = p.TuitionExpense,
                             LivingExpense = p.LivingExpense,
                             DependentExpense = p.DependentExpense,
                             OtherExpense = p.OtherExpense,
                             ExpenseRemarks = p.ExpenseRemarks,
                             PersonalFunding = p.PersonalFunding,
                             SchoolFunding = p.SchoolFunding,
                             SchoolFundingRemarks = p.SchoolFundingRemarks,
                             OtherFunding = p.OtherFunding,
                             OtherFundingRemarks = p.OtherFundingRemarks,
                             EmploymentFunding = p.EmploymentFunding
                         });
            return query;
        }

        /// <summary>
        /// Creates a query to return all participantStudentVisitors in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The filtered and sorted query to retrieve participantStudentVisitors.</returns>
        public static IQueryable<ParticipantStudentVisitorDTO> CreateGetParticipantStudentVisitorsDTOQuery(EcaContext context, QueryableOperator<ParticipantStudentVisitorDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetParticipantStudentVisitorsDTOQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Creates a query to return all participantStudentVisitors for the project with the given id in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The filtered and sorted query to retrieve participantStudentVisitors.</returns>
        public static IQueryable<ParticipantStudentVisitorDTO> CreateGetParticipantStudentVisitorsDTOByProjectIdQuery(EcaContext context, int projectId, QueryableOperator<ParticipantStudentVisitorDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetParticipantStudentVisitorsDTOQuery(context).Where(x => x.ProjectId == projectId);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns the participantStudentVisitors by participant id 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantStudentVisitors</returns>
        public static IQueryable<ParticipantStudentVisitorDTO> CreateGetParticipantStudentVisitorDTOByIdQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetParticipantStudentVisitorsDTOQuery(context).
                Where(p => p.ParticipantId == participantId);
            return query;
        }
    }
}
