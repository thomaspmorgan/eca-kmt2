using System;
using System.Diagnostics.Contracts;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Service.Persons
{
    public class UpdatedPersonEduEmp
    {
        public UpdatedPersonEduEmp(User updator, int id, string title, string role, DateTimeOffset startDate, DateTimeOffset? endDate, SimpleOrganizationDTO organization, int? personOfEducationId, int? personOfProfessionId)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            this.Update = new Update(updator);
            this.ProfessionEducationId = id;
            this.Title = title;
            this.Role = role;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Organization = organization;
            this.PersonOfEducationId = personOfEducationId;
            this.PersonOfProfessionId = personOfProfessionId;
        }

        public Update Update { get; private set; }

        public int ProfessionEducationId { get; private set; }
        
        public string Title { get; private set; }

        public string Role { get; private set; }

        public DateTimeOffset StartDate { get; private set; }

        public DateTimeOffset? EndDate { get; private set; }

        public SimpleOrganizationDTO Organization { get; private set; }

        public int? PersonOfEducationId { get; private set; }

        public int? PersonOfProfessionId { get; private set; }
    }
}
