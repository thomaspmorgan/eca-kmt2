using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The binding model for bookmark
    /// </summary>
    public class BookmarkBindingModel
    {
        /// <summary>
        /// Gets or sets office id
        /// </summary>
        public int? OfficeId { get; set; }

        /// <summary>
        /// Gets or sets program id 
        /// </summary>
        public int? ProgramId { get; set; }

        /// <summary>
        /// Gets or sets project id 
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// Gets or sets person id
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Gets or sets organization id
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets automatic flag
        /// </summary>
        public bool Automatic { get; set; }

        /// <summary>
        /// Convert from binding model to business model
        /// </summary>
        /// <param name="user">User creating the bookmark</param>
        /// <returns></returns>
        public NewBookmark ToNewBookmark(User user)
        {
            return new NewBookmark(this.OfficeId, this.ProgramId, this.PersonId, this.ProjectId, this.OrganizationId, user.Id, this.Automatic);
        }
    }
}