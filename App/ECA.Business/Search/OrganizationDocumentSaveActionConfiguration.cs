using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// The OrganizationDocumentSaveActionConfiguration is used to exclude offices from a document save action.
    /// </summary>
    public class OrganizationDocumentSaveActionConfiguration : DocumentSaveActionConfiguration<Organization>
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public OrganizationDocumentSaveActionConfiguration()
        {
            Func<Organization, bool> isNotOrganizationDelegate = (org) =>
            {
                return Organization.OFFICE_ORGANIZATION_TYPE_IDS.Contains(org.OrganizationTypeId);
            };
            var rules = new List<Func<Organization, bool>>();
            rules.Add(isNotOrganizationDelegate);

            this.CreatedExclusionRules = rules;
            this.DeletedExclusionRules = rules;
            this.ModifiedExclusionRules = rules;
        }
    }
}
