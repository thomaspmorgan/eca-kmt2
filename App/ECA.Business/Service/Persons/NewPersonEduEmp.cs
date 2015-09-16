using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Service.Persons
{
    public class NewPersonEduEmp
    {
        public NewPersonEduEmp(User user, string title, string role, DateTimeOffset startDate, DateTimeOffset? endDate, OrganizationDTO organization, int personId, Person personOfEducation = null, Person personOfProfession = null)
        {
            this.PersonId = personId;
            this.Title = title;
            this.Role = role;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Organization = organization;
            this.PersonOfEducation = personOfEducation;
            this.PersonOfProfession = personOfProfession;
            this.Create = new Create(user);
        }

        /// <summary>
        /// Gets/sets the person id.
        /// </summary>
        public int PersonId { get; private set; }

        public string Title { get; private set; }

        public string Role { get; private set; }

        public DateTimeOffset StartDate { get; private set; }

        public DateTimeOffset? EndDate { get; private set; }

        public OrganizationDTO Organization { get; private set; }

        public Person PersonOfEducation { get; private set; }

        public Person PersonOfProfession { get; private set; }


        public Create Create { get; private set; }

        public ProfessionEducation AddPersonEducation(Person person)
        {
            Contract.Requires(person != null, "The education entity must not be null.");
            var eduemp = new ProfessionEducation
            {
                Title = this.Title,
                Role = this.Role,
                DateFrom = this.StartDate,
                DateTo = this.EndDate,
                OrganizationId = this.Organization.OrganizationId,
                PersonOfEducation = this.PersonOfEducation
            };
            this.Create.SetHistory(eduemp);
            person.EducationalHistory.Add(eduemp);
            return eduemp;
        }

        public ProfessionEducation AddPersonProfession(Person person)
        {
            Contract.Requires(person != null, "The profession entity must not be null.");
            var eduemp = new ProfessionEducation
            {
                Title = this.Title,
                Role = this.Role,
                DateFrom = this.StartDate,
                DateTo = this.EndDate,
                OrganizationId = this.Organization.OrganizationId,
                PersonOfProfession = this.PersonOfProfession
            };
            this.Create.SetHistory(eduemp);
            person.ProfessionalHistory.Add(eduemp);
            return eduemp;
        }


    }
}
