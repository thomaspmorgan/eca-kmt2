using ECA.Data;
using System;
using System.Diagnostics.Contracts;

namespace ECA.Business.Service.Persons
{
    public class NewPersonEduEmp
    {
        public NewPersonEduEmp(User user, string title, string role, DateTimeOffset startDate, DateTimeOffset? endDate, int? organizationId, int? personOfEducation_PersonId, int? personOfProfession_PersonId, int personId)
        {
            PersonId = personId;
            Title = title;
            Role = role;
            StartDate = startDate;
            EndDate = endDate;
            OrganizationId = organizationId;
            PersonOfEducation_PersonId = personOfEducation_PersonId;
            PersonOfProfession_PersonId = personOfProfession_PersonId;
            Create = new Create(user);
        }

        /// <summary>
        /// Gets/sets the person id.
        /// </summary>
        public int PersonId { get; private set; }

        public string Title { get; private set; }

        public string Role { get; private set; }

        public DateTimeOffset StartDate { get; private set; }

        public DateTimeOffset? EndDate { get; private set; }

        public int? OrganizationId { get; private set; }

        public int? PersonOfEducation_PersonId { get; private set; }

        public int? PersonOfProfession_PersonId { get; private set; }


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
                OrganizationId = this.OrganizationId,
                PersonOfEducation_PersonId = this.PersonOfEducation_PersonId,
                PersonOfProfession_PersonId = null
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
                OrganizationId = this.OrganizationId,
                PersonOfEducation_PersonId = null,
                PersonOfProfession_PersonId = this.PersonOfProfession_PersonId
            };
            this.Create.SetHistory(eduemp);
            person.ProfessionalHistory.Add(eduemp);
            return eduemp;
        }


    }
}
