using ECA.Business.Models;
using ECA.Business.Service.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Projects
{
    /// <summary>
    /// A Draft Project is a new project that is in the draft state.
    /// </summary>
    public class DraftProjectBindingModel
    {
        /// <summary>
        /// The program id of the program this draft project belonds to.
        /// </summary>
        [Required]
        public int ProgramId { get; set; }

        /// <summary>
        /// The name of the draft project.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The description of the draft project.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Returns a DraftProject instance to be used by the ProjectService.
        /// </summary>
        /// <param name="userId">The id of the user creating the project.</param>
        public DraftProject ToDraftProject(int userId)
        {
            return new DraftProject(new ECA.Business.Service.User(userId), 
                                    this.Name, 
                                    this.Description, 
                                    this.ProgramId);
        }
    }
}