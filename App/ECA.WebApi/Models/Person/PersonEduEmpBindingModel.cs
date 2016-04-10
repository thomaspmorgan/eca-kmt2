using System;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// 
    /// </summary>
    public class PersonEduEmpBindingModel
    {
        //[Required]
        /// <summary>
        /// 
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? OrganizationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? PersonOfEducation_PersonId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? PersonOfProfession_PersonId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
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