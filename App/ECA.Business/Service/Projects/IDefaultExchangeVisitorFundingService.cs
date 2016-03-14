using ECA.Business.Queries.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    public interface IDefaultExchangeVisitorFundingService
    {
        Task<DefaultExchangeVisitorFundingDTO> GetDefaultExchangeVisitorFundingAsync(int projectId);
    }
}
