using System;
using System.Diagnostics.Contracts;

namespace ECA.Business.Service.Persons
{
    public class UpdatedPersonEduEmp
    {
        public UpdatedPersonEduEmp(User updator, int id, string title, string role, DateTimeOffset startDate, DateTimeOffset? endDate, int? organizationId, int? personOfEducationId, int? personOfProfessionId)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            this.Update = new Update(updator);
            this.ProfessionEducationId = id;
            this.Title = title;
            this.Role = role;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.OrganizationId = organizationId;
            this.PersonOfEducationId = personOfEducationId;
            this.PersonOfProfessionId = personOfProfessionId;
        }

        public Update Update { get; private set; }

        public int ProfessionEducationId { get; private set; }
        
        public string Title { get; private set; }

        public string Role { get; private set; }

        public DateTimeOffset StartDate { get; private set; }

        public DateTimeOffset? EndDate { get; private set; }
        
        public int? OrganizationId { get; private set; }

        public int? PersonOfEducationId { get; private set; }

        public int? PersonOfProfessionId { get; private set; }
    }
}
