using ECA.Business.Queries.Models.Admin;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Projects
{
    public static class DefaultExchangeVisitorFundingQueries
    {
        public static IQueryable<DefaultExchangeVisitorFundingDTO> CreateGetDefaultExchangeVisitorFundingDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = (from p in context.DefaultExchangeVisitorFunding
                         select new DefaultExchangeVisitorFundingDTO
                         {
                             ProjectId = p.ProjectId,
                             FundingGovtAgency1 = p.FundingGovtAgency1 ?? 0,
                             GovtAgency1Id = p.GovtAgency1Id ?? 0,
                             GovtAgency1Name = p.GovtAgency1 != null ? p.GovtAgency1.Description : string.Empty,
                             GovtAgency1OtherName = p.GovtAgency1OtherName != null ? p.GovtAgency1OtherName : string.Empty,
                             FundingGovtAgency2 = p.FundingGovtAgency2 ?? 0,
                             GovtAgency2Id = p.GovtAgency2Id ?? 0,
                             GovtAgency2Name = p.GovtAgency2 != null ? p.GovtAgency2.Description : string.Empty,
                             GovtAgency2OtherName = p.GovtAgency2OtherName != null ? p.GovtAgency2OtherName : string.Empty,
                             FundingIntlOrg1 = p.FundingIntlOrg1 ?? 0,
                             IntlOrg1Id = p.IntlOrg1Id ?? 0,
                             IntlOrg1Name = p.IntlOrg1 != null ? p.IntlOrg1.Description : string.Empty,
                             IntlOrg1OtherName = p.IntlOrg1OtherName != null ? p.IntlOrg1OtherName : string.Empty,
                             FundingIntlOrg2 = p.FundingIntlOrg2 ?? 0,
                             IntlOrg2Id = p.IntlOrg2Id ?? 0,
                             IntlOrg2Name = p.IntlOrg2 != null ? p.IntlOrg2.Description : string.Empty,
                             IntlOrg2OtherName = p.IntlOrg2OtherName != null ? p.IntlOrg2OtherName : string.Empty,
                             FundingOther = p.FundingOther ?? 0,
                             OtherName = p.OtherName != null ? p.OtherName : string.Empty,
                             FundingPersonal = p.FundingPersonal ?? 0,
                             FundingSponsor = p.FundingSponsor ?? 0,
                             FundingTotal = p.FundingTotal ?? 0,
                             FundingVisBNC = p.FundingVisBNC ?? 0,
                             FundingVisGovt = p.FundingVisGovt ?? 0
                         });
            return query;
        }

        public static IQueryable<DefaultExchangeVisitorFundingDTO> CreateGetDefaultExchangeVisitorFundingByIdQuery(EcaContext context, int projectId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetDefaultExchangeVisitorFundingDTOQuery(context)
                .Where(p => p.ProjectId == projectId);
            return query;
        }
    }
}
