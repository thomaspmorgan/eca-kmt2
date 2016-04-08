using System;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdatedPersonEduEmpBindingModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int ProfessionEducationId { get; set; }

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
        public UpdatedPersonEduEmp ToUpdatedPersonEduEmp(User user)
        {
            return new UpdatedPersonEduEmp(
                updator: user,
                professionEducationId: this.ProfessionEducationId,
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