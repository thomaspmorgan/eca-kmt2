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
using ECA.Core.Exceptions;

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

        public async Task UpdateAsync(UpdatedDefaultExchangeVisitorFunding updatedDefaultExchangeVisitorFunding)
        {
            var defaultExchangeVisitorFunding = await Context.DefaultExchangeVisitorFunding.FindAsync(updatedDefaultExchangeVisitorFunding.ProjectId);
            if (defaultExchangeVisitorFunding == null)
            {
                throw new ModelNotFoundException(String.Format("The default exchange visitor funding with id [{0}] was not found.", updatedDefaultExchangeVisitorFunding.ProjectId));
            }
            DoUpdate(defaultExchangeVisitorFunding, updatedDefaultExchangeVisitorFunding);
        }

        private void DoUpdate(DefaultExchangeVisitorFunding defaultExchangeVisitorFunding, UpdatedDefaultExchangeVisitorFunding updatedDefaultExchangeVisitorFunding)
        {
            updatedDefaultExchangeVisitorFunding.Audit.SetHistory(defaultExchangeVisitorFunding);

            defaultExchangeVisitorFunding.FundingSponsor = updatedDefaultExchangeVisitorFunding.FundingSponsor;
            defaultExchangeVisitorFunding.FundingPersonal = updatedDefaultExchangeVisitorFunding.FundingPersonal;
            defaultExchangeVisitorFunding.FundingVisGovt = updatedDefaultExchangeVisitorFunding.FundingVisGovt;
            defaultExchangeVisitorFunding.FundingVisBNC = updatedDefaultExchangeVisitorFunding.FundingVisBNC;
            defaultExchangeVisitorFunding.FundingGovtAgency1 = updatedDefaultExchangeVisitorFunding.FundingGovtAgency1;
            defaultExchangeVisitorFunding.GovtAgency1Id = updatedDefaultExchangeVisitorFunding.GovtAgency1Id;
            defaultExchangeVisitorFunding.GovtAgency1OtherName = updatedDefaultExchangeVisitorFunding.GovtAgency1OtherName;
            defaultExchangeVisitorFunding.FundingGovtAgency2 = updatedDefaultExchangeVisitorFunding.FundingGovtAgency2;
            defaultExchangeVisitorFunding.GovtAgency2Id = updatedDefaultExchangeVisitorFunding.GovtAgency2Id;
            defaultExchangeVisitorFunding.GovtAgency2OtherName = updatedDefaultExchangeVisitorFunding.GovtAgency2OtherName;
            defaultExchangeVisitorFunding.FundingIntlOrg1 = updatedDefaultExchangeVisitorFunding.FundingIntlOrg1;
            defaultExchangeVisitorFunding.IntlOrg1Id = updatedDefaultExchangeVisitorFunding.IntlOrg1Id;
            defaultExchangeVisitorFunding.IntlOrg1OtherName = updatedDefaultExchangeVisitorFunding.IntlOrg1OtherName;
            defaultExchangeVisitorFunding.FundingIntlOrg2 = updatedDefaultExchangeVisitorFunding.FundingIntlOrg2;
            defaultExchangeVisitorFunding.IntlOrg2Id = updatedDefaultExchangeVisitorFunding.IntlOrg2Id;
            defaultExchangeVisitorFunding.IntlOrg2OtherName = updatedDefaultExchangeVisitorFunding.IntlOrg2OtherName;
            defaultExchangeVisitorFunding.FundingOther = updatedDefaultExchangeVisitorFunding.FundingOther;
            defaultExchangeVisitorFunding.OtherName = updatedDefaultExchangeVisitorFunding.OtherName;
            defaultExchangeVisitorFunding.FundingTotal = updatedDefaultExchangeVisitorFunding.FundingTotal;
        }
    }
}
