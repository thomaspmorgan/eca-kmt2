using System;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    public class PersonEduEmpBindingModel
    {
        //[Required]
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
                personOfEducationPersonId: this.PersonOfEducation_PersonId,
                personOfProfessionPersonId: this.PersonOfProfession_PersonId,
                personId: this.PersonId
                );
        }
    }
}