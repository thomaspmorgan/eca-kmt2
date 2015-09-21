using System;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    public class UpdatedPersonEduEmpBindingModel
    {
        public int ProfessionEducationId { get; set; }

        public string Title { get; set; }

        public string Role { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public int? OrganizationId { get; set; }

        public int? PersonOfEducation_PersonId { get; set; }

        public int? PersonOfProfession_PersonId { get; set; }
        
        public UpdatedPersonEduEmp ToUpdatedPersonEduEmp(User user)
        {
            return new UpdatedPersonEduEmp(
                updator: user,
                id: this.ProfessionEducationId,
                title: this.Title,
                role: this.Role,
                startDate: this.StartDate,
                endDate: this.EndDate,
                organizationId: this.OrganizationId,
                personOfEducationId: this.PersonOfEducation_PersonId,
                personOfProfessionId: this.PersonOfProfession_PersonId
                );
        }

    }
}