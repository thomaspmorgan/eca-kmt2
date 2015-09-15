using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using ECA.Data;

namespace ECA.Business.Service.Persons
{
    public class UpdatedPersonEduEmp
    {
        public UpdatedPersonEduEmp(User updator, int id, string title, string role, DateTimeOffset startDate, DateTimeOffset? endDate, Organization organization)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            this.Update = new Update(updator);
            this.ProfessionEducationId = id;
            this.Title = title;
            this.Role = role;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Organization = organization;

        }

        public Update Update { get; private set; }

        public int ProfessionEducationId { get; private set; }
        
        public string Title { get; private set; }

        public string Role { get; private set; }

        public DateTimeOffset StartDate { get; private set; }

        public DateTimeOffset? EndDate { get; private set; }

        public Organization Organization { get; private set; }
    }
}
