using System;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.Query;
using System.Threading.Tasks;
using ECA.Core.DynamicLinq;
using System.Linq;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An IParticipantPersonSevisService is capable of performing crud operations on participants and their SEVIS information.
    /// </summary>
    public interface IParticipantStudentVisitorService
    {
        /// <summary>
        /// Returns the participantStudentVisitors in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantStudentVisitors.</returns>
        PagedQueryResults<ParticipantStudentVisitorDTO> GetParticipantStudentVisitors(QueryableOperator<ParticipantStudentVisitorDTO> queryOperator);

        /// <summary>
        /// Returns the participantStudentVisitors in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantStudentVisitors.</returns>
        Task<PagedQueryResults<ParticipantStudentVisitorDTO>> GetParticipantStudentVisitorsAsync(QueryableOperator<ParticipantStudentVisitorDTO> queryOperator);

        /// <summary>
        /// Returns the participantStudentVisitors for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantStudentVisitors.</returns>
        PagedQueryResults<ParticipantStudentVisitorDTO> GetParticipantStudentVisitorsByProjectId(int projectId, QueryableOperator<ParticipantStudentVisitorDTO> queryOperator);

        /// <summary>
        /// Returns the participantStudentVisitors for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantStudentVisitors.</returns>
        Task<PagedQueryResults<ParticipantStudentVisitorDTO>> GetParticipantStudentVisitorsByProjectIdAsync(int projectId, QueryableOperator<ParticipantStudentVisitorDTO> queryOperator);

        /// <summary>
        /// Returns the participantStudentVisitor by id
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantStudentVisitor</returns>
        ParticipantStudentVisitorDTO GetParticipantStudentVisitorById(int participantId);

        /// <summary>
        /// Returns the participantStudentVisitor by id asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantStudentVisitor</returns>
        Task<ParticipantStudentVisitorDTO> GetParticipantStudentVisitorByIdAsync(int participantId);

    }
}
