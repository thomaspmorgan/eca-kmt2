using ECA.Business.Validation.Sevis;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An IExchangeVisitorService is used to retrieve exchange visitors from the system.
    /// </summary>
    public interface IExchangeVisitorService
    {
        /// <summary>
        /// Returns the exchange visitor information for the participant in the given project.
        /// </summary>
        /// <param name="user">The user requesting the exchange visitor.</param>
        /// <param name="projectId">The project by id of the participant.</param>
        /// <param name="participantId">The participant by id.</param>
        /// <returns>The exchange visitor instance.  Use this instance for validation and serialization to sevis.</returns>
        ExchangeVisitor GetExchangeVisitor(User user, int projectId, int participantId);

        /// <summary>
        /// Returns the exchange visitor information for the participant in the given project.
        /// </summary>
        /// <param name="user">The user requesting the exchange visitor.</param>
        /// <param name="projectId">The project by id of the participant.</param>
        /// <param name="participantId">The participant by id.</param>
        /// <returns>The exchange visitor instance.  Use this instance for validation and serialization to sevis.</returns>
        Task<ExchangeVisitor> GetExchangeVisitorAsync(User user, int projectId, int participantId);
    }
}