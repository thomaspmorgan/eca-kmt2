using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Programs
{
    /// <summary>
    /// A ProgramViewModel is client side representation of a program .
    /// </summary>
    public class ProgramViewModel
    {
        /// <summary>
        /// Creates a new default instance and initializes IEnumerable properties.
        /// </summary>
        public ProgramViewModel()
        {
            this.Contacts = new List<SimpleLookupDTO>();
            this.CountryIsos = new List<SimpleLookupDTO>();
            this.RegionIsos = new List<SimpleLookupDTO>();
            this.Goals = new List<SimpleLookupDTO>();
            this.Themes = new List<SimpleLookupDTO>();
            this.Categories = new List<FocusCategoryDTO>();
            this.Objectives = new List<JustificationObjectiveDTO>();
            this.Regions = new List<LocationDTO>();
        }

        /// <summary>
        /// Creates a new ProgramViewModel and initializes itself with the given program dto.
        /// </summary>
        /// <param name="program">The program dto.</param>
        public ProgramViewModel(ProgramDTO program) : this()
        {
            Contract.Requires(program != null, "The program must not be null.");
            Contract.Requires(program.RowVersion != null, "The row version must not be null.");
            this.Contacts = program.Contacts;
            this.CountryIsos = program.CountryIsos;
            this.Description = program.Description;
            this.Goals = program.Goals;
            this.Id = program.Id;
            this.Name = program.Name;
            this.OwnerDescription = program.OwnerDescription;
            this.OwnerName = program.OwnerName;
            this.OwnerOrganizationId = program.OwnerOrganizationId;
            this.OwnerOfficeSymbol = program.OwnerOfficeSymbol;
            this.OwnerOfficeCategoryLabel = program.OwnerOrganizationCategoryLabel;
            this.OwnerOfficeObjectiveLabel = program.OwnerOrganizationObjectiveLabel;
            this.ParentProgramId = program.ParentProgramId;
            this.ParentProgramName = program.ParentProgramName;
            this.RegionIsos = program.RegionIsos;
            this.RevisedOn = program.RevisedOn;
            this.RowVersion = Convert.ToBase64String(program.RowVersion);
            this.StartDate = program.StartDate;
            this.EndDate = program.EndDate;
            this.Themes = program.Themes;
            this.Categories = program.Categories;
            this.Objectives = program.Objectives;
            this.ProgramStatusId = program.ProgramStatusId;
            this.ProgramStatusName = program.ProgramStatusName;
            this.Website = program.Website;
            this.Regions = program.Regions;
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Start Date.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets or sets the End Date.
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// Gets or sets the Revised On date.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the Parent Program Id.
        /// </summary>
        public int? ParentProgramId { get; set; }

        /// <summary>
        /// Gets or sets the Parent Program Name.
        /// </summary>
        public string ParentProgramName { get; set; }

        /// <summary>
        /// Gets or sets the Themes.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Themes { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        public IEnumerable<FocusCategoryDTO> Categories { get; set; }

        /// <summary>
        /// Gets or sets the objectives.
        /// </summary>
        public IEnumerable<JustificationObjectiveDTO> Objectives { get; set; }

        /// <summary>
        /// Gets or sets the Country Isos.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> CountryIsos { get; set; }

        /// <summary>
        /// Gets or sets the Region Isos.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> RegionIsos { get; set; }

        /// <summary>
        /// Gets or sets the Regions.
        /// </summary>
        public IEnumerable<LocationDTO> Regions { get; set; }

        /// <summary>
        /// Gets or sets the Goals.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Goals { get; set; }

        /// <summary>
        /// Gets or sets the Contacts.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the Owner Name.
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Gets or sets the owner description.
        /// </summary>
        public string OwnerDescription { get; set; }

        /// <summary>
        /// Gets or sets the owner organization id.
        /// </summary>
        public int OwnerOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the owner office symbol.
        /// </summary>
        public string OwnerOfficeSymbol { get; set; }

        public string OwnerOfficeCategoryLabel { get; set; }

        public string OwnerOfficeObjectiveLabel { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        public string RowVersion { get; set; }

        /// <summary>
        /// Gets or sets the program status id.
        /// </summary>
        public int ProgramStatusId { get; set; }

        /// <summary>
        /// Gets or sets the program status name.
        /// </summary>
        public string ProgramStatusName { get; set; }

        public string Website { get; set; }
    }
}