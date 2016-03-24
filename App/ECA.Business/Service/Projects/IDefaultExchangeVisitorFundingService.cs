using ECA.Business.Queries.Models.Admin;
using ECA.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    public interface IDefaultExchangeVisitorFundingService : ISaveable
    {
        Task<DefaultExchangeVisitorFundingDTO> GetDefaultExchangeVisitorFundingAsync(int projectId);
        Task UpdateAsync(UpdatedDefaultExchangeVisitorFunding updatedDefaultExchangeVisitorFunding);
    }
}
