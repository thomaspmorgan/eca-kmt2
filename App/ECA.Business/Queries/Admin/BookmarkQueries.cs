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
            var query = from bookmark in context.Bookmarks
                        let hasOffice = bookmark.OfficeId.HasValue
                        let office = OfficeQueries.CreateGetOfficeByIdQuery(context, bookmark.OfficeId.Value).FirstOrDefault()
                        let hasProgram = bookmark.ProgramId.HasValue
                        let program = ProgramQueries.CreateGetPublishedProgramByIdQuery(context, bookmark.ProgramId.Value).FirstOrDefault()
                        let hasProject = bookmark.ProjectId.HasValue
                        let project = ProjectQueries.CreateGetProjectByIdQuery(context, bookmark.ProjectId.Value).FirstOrDefault()
                        let hasPerson = bookmark.PersonId.HasValue
                        let person = PersonQueries.CreateGetSimplePersonDTOByPersonIdQuery(context, bookmark.PersonId.Value).FirstOrDefault()
                        let hasOrganization = bookmark.OrganizationId.HasValue
                        let organization = OrganizationQueries.CreateGetOrganizationDTOByOrganizationIdQuery(context, bookmark.OrganizationId.Value).FirstOrDefault()
                        select new BookmarkDTO
                        {
                            BookmarkId = bookmark.BookmarkId,
                            OfficeId = hasProject ? project.OwnerId : bookmark.OfficeId,
                            ProgramId = hasProgram ? program.OwnerOrganizationId : bookmark.ProgramId,
                            ProjectId = bookmark.ProjectId,
                            PersonId = bookmark.PersonId,
                            OrganizationId = bookmark.OrganizationId,
                            PrincipalId = bookmark.PrincipalId,
                            AddedOn = bookmark.AddedOn,
                            Automatic = bookmark.Automatic,
                            Type = hasOffice ? "Office" :
                                   hasProgram != null ? "Program" :
                                   hasProject != null ? "Project" :
                                   hasPerson != null ? "Person" :
                                                        "Organization",
                            OfficeSymbolOrStatus = hasOffice ? office.OfficeSymbol :
                                                   hasProgram ? program.OwnerOfficeSymbol :
                                                   hasProject ? project.OwnerOfficeSymbol :
                                                   hasPerson ? person.CurrentStatus :
                                                               organization.Status,
                            Name = hasOffice ? office.Name :
                                   hasProgram ? program.Name :
                                   hasProject ? project.Name :
                                   hasPerson ? person.FullName :
                                   organization.Name

                        };
            return query;
        }
    }
}
