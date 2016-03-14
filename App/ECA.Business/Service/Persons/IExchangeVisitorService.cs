using ECA.Business.Validation.Sevis;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    public interface IExchangeVisitorService
    {
        ExchangeVisitor GetExchangeVisitor(User user, int projectId, int participantId);

        Task<ExchangeVisitor> GetExchangeVisitorAsync(User user, int projectId, int participantId);
    }
}