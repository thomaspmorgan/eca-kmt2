using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Persons;
using ECA.Business.Queries.Programs;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Admin
{
    /// <summary>
    /// Queries for bookmark service
    /// </summary>
    public static class BookmarkQueries
    {
        /// <summary>
        /// Gets a list of bookmark dtos 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <returns>Bookmark dtos</returns>
        public static IQueryable<BookmarkDTO> CreateGetBookmarksQuery(EcaContext context)
        {
            var allOffices = OfficeQueries.CreateGetOfficesQuery(context);
            var allPrograms = ProgramQueries.CreateGetPublishedProgramsQuery(context);
            var allProjects = ProjectQueries.CreateGetProjectsQuery(context);
            var allPeople = PersonQueries.CreateGetSimplePersonDTOsQuery(context);
            var allOrganizations = OrganizationQueries.CreateGetSimpleOrganizationsDTOQuery(context);

            var query = from bookmark in context.Bookmarks

                        let hasOffice = bookmark.OfficeId.HasValue
                        let office = allOffices.Where(x => x.Id == bookmark.OfficeId).FirstOrDefault()
                        let hasProgram = bookmark.ProgramId.HasValue
                        let program = allPrograms.Where(x => x.Id == bookmark.ProgramId).FirstOrDefault()
                        let hasProject = bookmark.ProjectId.HasValue
                        let project = allProjects.Where(x => x.ProjectId == bookmark.ProjectId).FirstOrDefault()
                        let hasPerson = bookmark.PersonId.HasValue
                        let person = allPeople.Where(x => x.PersonId == bookmark.PersonId).FirstOrDefault()
                        let hasOrganization = bookmark.OrganizationId.HasValue
                        let organization = allOrganizations.Where(x => x.OrganizationId == bookmark.OrganizationId).FirstOrDefault()

                        select new BookmarkDTO
                        {
                            BookmarkId = bookmark.BookmarkId,
                            OfficeId = hasProject ? project.OwnerId : bookmark.OfficeId,
                            ProgramId = hasProject ? project.ProgramId : bookmark.ProgramId,
                            ProjectId = bookmark.ProjectId,
                            PersonId = bookmark.PersonId,
                            OrganizationId = bookmark.OrganizationId,
                            PrincipalId = bookmark.PrincipalId,
                            AddedOn = bookmark.AddedOn,
                            Automatic = bookmark.Automatic,
                            Type = hasProject ? "Project" :
                                   hasOffice ? "Office" :
                                   hasProgram ? "Program" :
                                   hasPerson ? "Person" :
                                   hasOrganization ? "Organization" : "Unknown",
                            OfficeSymbolOrStatus = hasProject ? project.OwnerOfficeSymbol :
                                                   hasOffice ? office.OfficeSymbol :
                                                   hasProgram ? program.OwnerOfficeSymbol :
                                                   hasPerson ? person.CurrentStatus :
                                                   hasOrganization ? organization.Status : "",
                            Name = hasProject ? project.ProjectName :
                                   hasOffice ? office.Name : 
                                   hasProgram ? program.Name :
                                   hasPerson ? person.FullName :
                                   hasOrganization ? organization.Name : ""
                        };
            return query;
        }
    }
}
