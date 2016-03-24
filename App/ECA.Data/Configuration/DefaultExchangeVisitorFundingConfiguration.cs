using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    class DefaultExchangeVisitorFundingConfiguration : EntityTypeConfiguration<DefaultExchangeVisitorFunding> 
    {
        public DefaultExchangeVisitorFundingConfiguration()
        {
            HasOptional(a => a.GovtAgency1).WithMany().HasForeignKey(a => a.GovtAgency1Id).WillCascadeOnDelete(false);
            HasOptional(a => a.GovtAgency2).WithMany().HasForeignKey(a => a.GovtAgency2Id).WillCascadeOnDelete(false);
            HasOptional(a => a.IntlOrg1).WithMany().HasForeignKey(a => a.IntlOrg1Id).WillCascadeOnDelete(false);
            HasOptional(a => a.IntlOrg2).WithMany().HasForeignKey(a => a.IntlOrg2Id).WillCascadeOnDelete(false);
        }
    }
}
