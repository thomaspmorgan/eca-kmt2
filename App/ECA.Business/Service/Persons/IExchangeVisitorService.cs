using System.Threading.Tasks;
using ECA.Business.Validation.Model;

namespace ECA.Business.Service.Persons
{
    public interface IExchangeVisitorService
    {
        CreateExchVisitor GetCreateExchangeVisitor(User user, int projectId, int participantId);
        Task<CreateExchVisitor> GetCreateExchangeVisitorAsync(User user, int projectId, int participantId);
        UpdateExchVisitor GetUpdateExchangeVisitor(User user, int projectId, int participantId);
        Task<UpdateExchVisitor> GetUpdateExchangeVisitorAsync(User user, int projectId, int participantId);
    }
}