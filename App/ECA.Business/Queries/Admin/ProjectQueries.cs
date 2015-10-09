using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace ECA.Business.Queries.Admin
{
    /// <summary>
    /// Contains queries related to an ECA project that must query the EcaContext.
    /// </summary>
    public static class ProjectQueries
    {
        /// <summary>
        /// Returns a query to retrieve simple projects.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query to retrieve the projects.</returns>
        public static IQueryable<SimpleProjectDTO> CreateGetProjectsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var countryQuery = from country in context.Locations
                               where country.LocationTypeId == LocationType.Country.Id
                               select country;

            var query = from project in context.Projects
                        let parentProgram = project.ParentProgram
                        let categories = project.Categories
                        let objectives = project.Objectives
                        let status = project.Status
                        let startDate = project.StartDate
                        let regions = project.Regions
                        let countries = countryQuery.Where(x => regions.Select(y => y.LocationId).Contains(x.Region.LocationId))

                        select new SimpleProjectDTO
                        {
                            CountryIds = countries.Select(x => x.LocationId),
                            CountryNames = countries.Select(x => x.LocationName),
                            ProgramId = parentProgram.ProgramId,
                            ProgramName = parentProgram.Name,
                            ProjectId = project.ProjectId,
                            ProjectName = project.Name,
                            ProjectStatusId = status.ProjectStatusId,
                            ProjectStatusName = status.Status,
                            RegionIds = regions.Select(x => x.LocationId),
                            RegionNames = regions.Select(x => x.LocationName),
                            StartDate = startDate,
                            StartYear = startDate.Year,
                            StartYearAsString = startDate.Year.ToString(),
                            OwnerId = parentProgram.OwnerId,
                            OwnerOfficeSymbol = parentProgram.Owner != null ? parentProgram.Owner.OfficeSymbol : null
                        };

            return query;
        }

        /// <summary>
        /// Returns a query to retrieve simple projects that have been filtered and sorted.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to retrieve the filtered and sorted projects.</returns>
        public static IQueryable<SimpleProjectDTO> CreateGetProjectsQuery(EcaContext context, QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetProjectsQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns a query to retrieve filtered and sorted simple project dtos for the given program id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="programId">The project's parent program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to retrieved filtered and sorted simple project dtos for the given program id.</returns>
        public static IQueryable<SimpleProjectDTO> CreateGetProjectsByProgramQuery(EcaContext context, int programId, QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetProjectsQuery(context).Where(x => x.ProgramId == programId);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns projects by person id
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="personId">The person id</param>
        /// <param name="queryOperator">The query operator</param>
        /// <returns>Return projects by person id</returns>
        public static IQueryable<ParticipantTimelineDTO> CreateGetProjectsByPersonIdQuery(EcaContext context, int personId, QueryableOperator<ParticipantTimelineDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");

            var query = context.Participants.Where(x => x.PersonId == personId).Select(x => new ParticipantTimelineDTO
            {
                ProjectId = x.ProjectId,
                ParticipantId = x.ParticipantId,
                ProgramId = x.Project.ProgramId,
                OfficeId = x.Project.ParentProgram != null ? x.Project.ParentProgram.OwnerId : default(int),
                Name = x.Project.Name,
                StartDate = x.Project.StartDate,
                EndDate = x.Project.EndDate,
                OfficeSymbol = x.Project.ParentProgram != null && x.Project.ParentProgram.Owner != null ? x.Project.ParentProgram.Owner.OfficeSymbol : null,
                Description = x.Project.Description,
                Status = x.Project.Status != null ? x.Project.Status.Status : null
            });
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns a project by id
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="projectId">The project id to fetch</param>
        /// <returns>Project</returns>
        public static IQueryable<ProjectDTO> CreateGetProjectByIdQuery(EcaContext context, int projectId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetProjectDTOQuery(context).Where(x => x.Id == projectId);
        }

        // <summary>
        /// Returns a query to retrieve projets.
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <returns>Project</returns>
        public static IQueryable<ProjectDTO> CreateGetProjectDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var countryTypeId = LocationType.Country.Id;
            var regionTypeId = LocationType.Region.Id;
            var allLocations = LocationQueries.CreateGetLocationsQuery(context);

            var query = from project in context.Projects
                        let program = project.ParentProgram
                        let owner = program.Owner
                        let status = project.Status
                        let themes = project.Themes
                        let regions = project.Regions
                        let goals = project.Goals
                        let contacts = project.Contacts
                        let categories = project.Categories
                        let objectives = project.Objectives


                        let locations = from location in allLocations
                                        join projectLocation in project.Locations
                                        on location.Id equals projectLocation.LocationId
                                        select location


                        let locationsWithCountries = (from location in allLocations

                                                      join projectLocation in project.Locations
                                                      on location.Id equals projectLocation.LocationId

                                                      join countryLocation in allLocations
                                                      on location.CountryId equals countryLocation.Id

                                                      where location.LocationTypeId != regionTypeId
                                                      && location.LocationTypeId != countryTypeId

                                                      select countryLocation)

                        let countries = (from location in allLocations

                                         join projectLocation in project.Locations
                                         on location.Id equals projectLocation.LocationId

                                         where location.LocationTypeId == countryTypeId

                                         select location)

                        let regionCountries = (from location in allLocations

                                               join projectLocation in project.Locations
                                               on location.Id equals projectLocation.LocationId

                                               join countryLocation in allLocations
                                               on location.Id equals countryLocation.RegionId

                                               where countryLocation.LocationTypeId == countryTypeId

                                               select countryLocation)

                        let allCountries = locationsWithCountries
                            .Union(countries)
                            .Union(regionCountries)
                        select new ProjectDTO
                        {
                            Id = project.ProjectId,
                            Name = project.Name,
                            Description = project.Description,
                            ProjectStatusId = status.ProjectStatusId,
                            Status = status.Status,
                            RevisedOn = project.History.RevisedOn,
                            StartDate = project.StartDate,
                            EndDate = project.EndDate,
                            ProgramId = project.ProgramId,
                            ProgramName = program.Name,
                            OwnerId = owner.OrganizationId,
                            OwnerName = owner.Name,
                            OwnerOfficeSymbol = owner.OfficeSymbol,
                            Themes = themes.Select(x => new SimpleLookupDTO { Id = x.ThemeId, Value = x.ThemeName }),
                            CountryIsos = allCountries.Select(x => new SimpleLookupDTO { Id = x.Id, Value = x.LocationIso }).Distinct(),
                            Locations = locations,
                            Goals = goals.Select(x => new SimpleLookupDTO { Id = x.GoalId, Value = x.GoalName }),
                            Contacts = contacts.Select(x => new SimpleLookupDTO { Id = x.ContactId, Value = x.FullName + " (" + x.Position + ")" }),
                            Objectives = objectives.Select(o => new JustificationObjectiveDTO { Id = o.ObjectiveId, Name = o.ObjectiveName, JustificationName = o.Justification.JustificationName }),
                            Categories = categories.Select(c => new FocusCategoryDTO { Id = c.CategoryId, Name = c.CategoryName, FocusName = c.Focus.FocusName }),
                        };
            return query;
        }
    }
}
