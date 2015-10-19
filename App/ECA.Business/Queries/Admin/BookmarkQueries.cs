using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Persons;
using ECA.Business.Queries.Programs;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
            Contract.Requires(context != null, "The context must not be null.");
            var allPeople = PersonQueries.CreateGetSimplePersonDTOsQuery(context);
            var query = from bookmark in context.Bookmarks

                        let hasOffice = bookmark.OfficeId.HasValue
                        let office = bookmark.Office

                        let hasProgram = bookmark.ProgramId.HasValue
                        let program = bookmark.Program

                        let hasProject = bookmark.ProjectId.HasValue
                        let project = bookmark.Project

                        let hasPerson = bookmark.PersonId.HasValue
                        let person = allPeople.Where(x => x.PersonId == bookmark.PersonId).FirstOrDefault()

                        let hasOrganization = bookmark.OrganizationId.HasValue
                        let organization = bookmark.Organization

                        let ownerSymbol = hasProject ? project.ParentProgram.Owner.OfficeSymbol : 
                                    hasProgram ? program.Owner.OfficeSymbol :
                                    hasOffice ? office.OfficeSymbol : "UNKNOWN OFFICE SYMBOL"


                        select new BookmarkDTO
                        {
                            BookmarkId = bookmark.BookmarkId,
                            OfficeId = bookmark.OfficeId,
                            ProgramId = bookmark.ProgramId,
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
                            OfficeSymbolOrStatus = (hasProject || hasProgram || hasOffice) ? ownerSymbol :
                                                   hasPerson ? person.CurrentStatus :
                                                   hasOrganization ? organization.Status : "",
                            Name = hasProject ? project.Name :
                                   hasOffice ? office.Name :
                                   hasProgram ? program.Name :
                                   hasPerson ? person.FullName :
                                   hasOrganization ? organization.Name : ""
                        };
            return query;
        }
    }
}
