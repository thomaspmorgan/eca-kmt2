using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Projects;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System.Diagnostics.Contracts;
using System.Data.Entity;

namespace ECA.Business.Service.Projects
{
    public class DefaultExchangeVisitorFundingService : DbContextService<EcaContext>, IDefaultExchangeVisitorFundingService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DefaultExchangeVisitorFundingService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        public Task<DefaultExchangeVisitorFundingDTO> GetDefaultExchangeVisitorFundingAsync(int projectId)
        {
            var dto = DefaultExchangeVisitorFundingQueries.CreateGetDefaultExchangeVisitorFundingByIdQuery(Context, projectId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved default exchange visitor funding by id [{0}].", projectId);
            return dto;
        }
    }
}
