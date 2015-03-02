using System;
namespace ECA.Business.Service.Persons
{
    public interface IContactService
    {
        ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.ContactDTO> GetContacts(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.ContactDTO> queryOperator);
        System.Threading.Tasks.Task<ECA.Core.Query.PagedQueryResults<ECA.Business.Queries.Models.Persons.ContactDTO>> GetContactsAsync(ECA.Core.DynamicLinq.QueryableOperator<ECA.Business.Queries.Models.Persons.ContactDTO> queryOperator);
    }
}
