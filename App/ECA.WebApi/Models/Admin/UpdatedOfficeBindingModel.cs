using ECA.Business.Service.Admin;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// Binding model for updating an office
    /// </summary>
    public class UpdatedOfficeBindingModel
    {
        /// <summary>
        /// Gets or sets the office id
        /// </summary>
        public int OfficeId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [Required]
        [MaxLength(Organization.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the office symbol
        /// </summary>
        [Required]
        [MaxLength(Organization.OFFICE_SYMBOL_MAX_LENGTH)]
        public string OfficeSymbol { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [Required]
        [MaxLength(Organization.DESCRIPTION_MAX_LENGTH)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the parent office id
        /// </summary>
        public int? ParentOfficeId { get; set; }

        /// <summary>
        /// Gets or sets the points of contact ids
        /// </summary>
        public IEnumerable<int> PointsOfContactIds { get; set; }
        
        /// <summary>
        /// Convert to a updated office business object
        /// </summary>
        /// <param name="user">The user updating the office</param>
        /// <returns>The updated office</returns>
        public UpdatedOffice ToUpdatedOffice(ECA.Business.Service.User user)
        {
            Contract.Requires(user != null, "The user must not be null.");
            return new UpdatedOffice(user, OfficeId, Name, OfficeSymbol, Description, ParentOfficeId, PointsOfContactIds);
        }
    }
}