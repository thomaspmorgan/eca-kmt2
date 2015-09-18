using System;
using System.ComponentModel.DataAnnotations;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    public class PersonEduEmpBindingModel
    {
        [Required]
        public int PersonId { get; set; }

        public string Title { get; set; }

        public string Role { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public int? OrganizationId { get; set; }

        public int? PersonOfEducation_PersonId { get; set; }

        public int? PersonOfProfession_PersonId { get; set; }

        public NewPersonEduEmp ToPersonEduEmp(User user)
        {
            return new NewPersonEduEmp(
                user: user,
                title: this.Title,
                role: this.Role,
                startDate: this.StartDate,
                endDate: this.EndDate,
                organizationId: this.OrganizationId,
                personId: this.PersonId,
                personOfEducation_PersonId: this.PersonOfEducation_PersonId,
                personOfProfession_PersonId: this.PersonOfProfession_PersonId
                );
        }
    }
}