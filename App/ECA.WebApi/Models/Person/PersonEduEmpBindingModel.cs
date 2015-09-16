using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ECA.Business.Service.Persons;
using ECA.Business.Service;
using ECA.Data;
using ECA.Business.Queries.Models.Admin;

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

        public OrganizationDTO Organization { get; set; }

        public Data.Person PersonOfEducation { get; set; }

        public Data.Person PersonOfProfession { get; set; }

        public NewPersonEduEmp ToPersonEduEmp(User user)
        {
            return new NewPersonEduEmp(
                user: user,
                title: this.Title,
                role: this.Role,
                startDate: this.StartDate,
                endDate: this.EndDate,
                organization: this.Organization,
                personId: this.PersonId,
                personOfEducation: this.PersonOfEducation,
                personOfProfession: this.PersonOfProfession
                );
        }
    }
}