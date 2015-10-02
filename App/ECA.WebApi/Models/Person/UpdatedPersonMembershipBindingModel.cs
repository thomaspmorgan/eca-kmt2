using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for editing membership
    /// </summary>
    public class UpdatedPersonMembershipBindingModel
    {
        /// <summary>
        /// Gets and sets the membership id
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Gets and sets the membership for the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Convert binding model to business model 
        /// </summary>
        /// <param name="user">The user updating the membership</param>
        /// <returns>Update membership business model</returns>
        public UpdatedPersonMembership ToUpdatedPersonMembership(User user)
        {
            return new UpdatedPersonMembership(
                updator: user,
                id: this.Id,
                name: this.Name
                );
        }
    }
}