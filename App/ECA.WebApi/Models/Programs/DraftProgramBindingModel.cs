using ECA.Business.Models.Programs;
using ECA.Business.Service.Programs;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Programs
{
    /// <summary>
    /// A DraftProgram is a program that is currently in draft state.
    /// </summary>
    public class DraftProgramBindingModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DraftProgramBindingModel()
        {
            this.Websites = new List<WebsiteBindingModel>();
        }

        /// <summary>
        /// The name of the program.
        /// </summary>
        [Required]
        [MaxLength(Program.MAX_NAME_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// The program description.
        /// </summary>
        [Required]
        [MaxLength(Program.MAX_DESCRIPTION_LENGTH)]
        public string Description { get; set; }

        /// <summary>
        /// The start date.
        /// </summary>
        [Required]
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// The end date.
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// The id of the owner organization.
        /// </summary>
        [Required]
        public int OwnerOrganizationId { get; set; }

        /// <summary>
        /// The parent program id.
        /// </summary>
        public int? ParentProgramId { get; set; }

        /// <summary>
        /// The goals by id.
        /// </summary>
        public List<int> Goals { get; set; }

        /// <summary>
        /// The themes by id.
        /// </summary>
        public List<int> Themes { get; set; }

        /// <summary>
        /// The points of contact by id.
        /// </summary>
        public List<int> Contacts { get; set; }

        /// <summary>
        /// The regions by id.
        /// </summary>
        public List<int> Regions { get; set; }

        /// <summary>
        /// The categories by id.
        /// </summary>
        public List<int> Categories { get; set; }

        /// <summary>
        /// The objectives by id.
        /// </summary>
        public List<int> Objectives { get; set; }

        /// <summary>
        /// Gets or sets the websites
        /// </summary>
        public List<WebsiteBindingModel> Websites { get; set; }

        /// <summary>
        /// Returns a DraftProgram business entity from this binding model.
        /// </summary>
        /// <param name="user">The user making the change.</param>
        /// <returns>The draft program.</returns>
        public DraftProgram ToDraftProgram(ECA.Business.Service.User user)
        {
            return new DraftProgram(
                createdBy: user,
                name: this.Name,
                description: this.Description,
                startDate: this.StartDate,
                endDate: this.EndDate,
                ownerOrganizationId: this.OwnerOrganizationId,
                parentProgramId: this.ParentProgramId,
                goalIds: this.Goals,
                pointOfContactIds: this.Contacts,
                themeIds: this.Themes,
                regionIds: this.Regions,
                categoryIds: this.Categories,
                objectiveIds: this.Objectives,
                websites: GetWebsites()
                );
        }

        /// <summary>
        /// Converts list of website binding models to website dtos
        /// </summary>
        /// <returns></returns>
        protected List<WebsiteDTO> GetWebsites()
        {
            List<WebsiteDTO> websites = new List<WebsiteDTO>();
            foreach(WebsiteBindingModel website in Websites)
            {
                websites.Add(new WebsiteDTO(website.Id, website.Value));
            }
            return websites;
        }
    }
}