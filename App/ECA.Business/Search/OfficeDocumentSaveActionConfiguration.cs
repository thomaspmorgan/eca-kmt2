using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// The OfficeDocumentSaveActionConfiguration is used to exclude organizations from a document save action.
    /// </summary>
    public class OfficeDocumentSaveActionConfiguration : DocumentSaveActionConfiguration<Organization>
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public OfficeDocumentSaveActionConfiguration()
            : base(
                  x => x.OrganizationId,
                  OfficeDTODocumentConfiguration.OFFICE_DTO_DOCUMENT_TYPE_ID
                  )
        {
            Func<Organization, bool> isNotOfficeDelegate = (org) =>
            {
                return !Organization.OFFICE_ORGANIZATION_TYPE_IDS.Contains(org.OrganizationTypeId);
            };
            var rules = new List<Func<Organization, bool>>();
            rules.Add(isNotOfficeDelegate);

            this.CreatedExclusionRules = rules;
            this.DeletedExclusionRules = rules;
            this.ModifiedExclusionRules = rules;
        }
    }
}
