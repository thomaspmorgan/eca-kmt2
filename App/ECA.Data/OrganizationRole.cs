using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    /// <summary>
    /// The model for an organization role
    /// </summary>
    public partial class OrganizationRole
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrganizationRole()
        {
            this.History = new History();
        }

        /// <summary>
        /// Gets and sets the organization role id
        /// </summary>
        [Key]
        public int OrganizationRoleId { get; set; }

        /// <summary>
        /// Gets and sets the organization role name
        /// </summary>
        [Required]
        public string OrganizationRoleName { get; set; }

        /// <summary>
        /// Gets and sets the history
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// Gets or sets the organizations
        /// </summary>
        public virtual ICollection<Organization> Organizations { get; set; }
    }
}
