using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The UpdateOrganizationValidationEntity object is used to validate organizations that will be updated in the ECA system.
    /// </summary>
    public class UpdateOrganizationValidationEntity
    {
        /// <summary>
        /// Creates a new instance with the validation parameters.
        /// </summary>
        /// <param name="name">The name of the organization.</param>
        public UpdateOrganizationValidationEntity(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the organization.
        /// </summary>
        public string Name { get; private set; }
    }
}
