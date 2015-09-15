using ECA.Business.Models;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Projects;
using ECA.Data;
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
        [MaxLength(Project.MAX_NAME_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// The description of the draft project.
        /// </summary>
        [Required]
        [MaxLength(Project.MAX_DESCRIPTION_LENGTH)]
        public string Description { get; set; }

        /// <summary>
        /// Returns a DraftProject instance to be used by the ProjectService.
        /// </summary>
        ///<param name="user">The user making the change.</param>
        public DraftProject ToDraftProject(ECA.Business.Service.User user)
        {
            return new DraftProject(user,
                                    this.Name, 
                                    this.Description, 
                                    this.ProgramId);
        }
    }
}