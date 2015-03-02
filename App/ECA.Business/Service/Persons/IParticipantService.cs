using System;
namespace ECA.Business.Service.Persons
{
    public interface IParticipantService
    {
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> GetParticipants(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> queryOperator);
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO>> GetParticipantsAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.SimpleParticipantDTO> queryOperator);
    }
}
