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
    public static class ProjectQueries
    {
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

                        where project.ProgramId == programId
                        select new SimpleProjectDTO
                        {
                            CountryIds = countries.Select(x => x.LocationId),
                            CountryNames = countries.Select(x => x.LocationName),
                            ProgramId = parentProgram.ProgramId,
                            ProjectId = project.ProjectId,
                            ProjectName = project.Name,
                            ProjectStatusId = status.ProjectStatusId,
                            ProjectStatusName = status.Status,
                            StartDate = startDate,
                            StartYear = startDate.Year,
                            StartYearAsString = startDate.Year.ToString()
                        };

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
            var countryQuery = from country in context.Locations
                               where country.LocationTypeId == LocationType.Country.Id
                               select country;

            var query = from project in context.Projects
                        let program = project.ParentProgram
                        let owner = program.Owner
                        let status = project.Status
                        let themes = project.Themes
                        let regions = project.Regions
                        let countries = countryQuery.Where(x => regions.Select(y => y.LocationId).Contains(x.Region.LocationId))
                        let goals = project.Goals
                        let contacts = project.Contacts
                        let categories = project.Categories
                        let objectives = project.Objectives
                        where project.ProjectId == projectId
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
                            Themes = themes.Select(x => new SimpleLookupDTO { Id = x.ThemeId, Value = x.ThemeName }),
                            CountryIsos = countries.Select(x => new SimpleLookupDTO { Id = x.LocationId, Value = x.LocationIso }),
                            Goals = goals.Select(x => new SimpleLookupDTO {Id = x.GoalId, Value = x.GoalName}),
                            Contacts = contacts.Select(x => new SimpleLookupDTO {Id = x.ContactId, Value = x.FullName + " (" + x.Position + ")"}),
                            Objectives = objectives.Select(o => new JustificationObjectiveDTO { Id = o.ObjectiveId, Name = o.ObjectiveName, JustificationName = o.Justification.JustificationName }),
                            Categories = categories.Select(c => new FocusCategoryDTO { Id = c.CategoryId, Name = c.CategoryName, FocusName = c.Focus.FocusName }),

                        };
                return query;

        }
    }
}
