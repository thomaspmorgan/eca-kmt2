using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Programs
{
    public static class ProgramQueries
    {
        /// <summary>
        /// Creates a query to return filtered and sorted simple program dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query to return filtered and sorted simple program dtos.</returns>
        public static IQueryable<SimpleProgramDTO> CreateGetSimpleProgramDTOsQuery(EcaContext context, QueryableOperator<SimpleProgramDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = context.Programs.Select(x => new SimpleProgramDTO
            {
                Description = x.Description,
                Name = x.Name,
                OwnerId = x.Owner.OrganizationId,
                ProgramId = x.ProgramId,
                ProgramStatusId = x.ProgramStatusId
            });
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns a query to locate a program by id.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="programId">The program id.</param>
        /// <returns>The program.</returns>
        public static IQueryable<ProgramDTO> CreateGetPublishedProgramByIdQuery(EcaContext context, int programId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetPublishedProgramsQuery(context).Where(x => x.Id == programId);
        }

        /// <summary>
        /// Returns an EcaProgram query.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The EcaProgram query.</returns>
        private static IQueryable<ProgramDTO> CreateGetPublishedProgramsQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var countryQuery = from country in context.Locations
                               where country.LocationTypeId == LocationType.Country.Id
                               select country;

            var query = from program in context.Programs
                        let owner = program.Owner
                        let themes = program.Themes
                        let regions = program.Regions
                        let parentProgram = program.ParentProgram
                        let goals = program.Goals
                        let contacts = program.Contacts
                        let categories = program.Categories
                        let objectives = program.Objectives
                        let countries = countryQuery.Where(c => regions.Contains(c.Region))

                        select new ProgramDTO
                        {
                            Contacts = contacts.Select(x => new SimpleLookupDTO { Id = x.ContactId, Value = x.FullName }),
                            CountryIsos = countries.Select(x => new SimpleLookupDTO { Id = x.LocationId, Value = x.LocationIso }),
                            Description = program.Description,
                            Categories = categories.Select(c => new FocusCategoryDTO { Id = c.CategoryId, Name = c.CategoryName, FocusName = c.Focus.FocusName}),
                            Goals = goals.Select(x => new SimpleLookupDTO { Id = x.GoalId, Value = x.GoalName }),
                            Id = program.ProgramId,
                            Objectives = objectives.Select(o => new JustificationObjectiveDTO { Id = o.ObjectiveId, Name = o.ObjectiveName, JustificationName = o.Justification.JustificationName}),
                            Name = program.Name,
                            OwnerDescription = owner.Description,
                            OwnerName = owner.Name,
                            OwnerOfficeSymbol = owner.OfficeSymbol,
                            OwnerOrganizationId = owner.OrganizationId,
                            OwnerOrganizationCategoryLabel = owner.OfficeSettings.FirstOrDefault(o => o.Name == "Focus").Value,
                            OwnerOrganizationObjectiveLabel = owner.OfficeSettings.FirstOrDefault(o => o.Name == "Justification").Value,
                            ParentProgramId = parentProgram == null ? default(int?) : parentProgram.ProgramId,
                            RevisedOn = program.History.RevisedOn,
                            RegionIsos = regions.Select(x => new SimpleLookupDTO { Id = x.LocationId, Value = x.LocationIso }),
                            RowVersion = program.RowVersion,
                            StartDate = program.StartDate,
                            Themes = themes.Select(x => new SimpleLookupDTO { Id = x.ThemeId, Value = x.ThemeName }),
                            ProgramStatusId = program.ProgramStatusId,
                            Website = program.Website
                        };
            return query;
        }
    }
}
