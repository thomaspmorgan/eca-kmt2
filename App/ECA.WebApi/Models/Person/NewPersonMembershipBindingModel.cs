using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for editing pii
    /// </summary>
    public class NewPersonMembershipBindingModel
    {
        /// <summary>
        /// Gets and sets the person id
        /// </summary>
        [Required]
        public int PersonId { get; set; }

        /// <summary>
        /// Gets and sets the membership for the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Convert binding model to business model 
        /// </summary>
        /// <param name="user">The user creating the membership</param>
        /// <returns>Create membership business model</returns>
        public NewPersonMembership ToPersonMembership(User user)
        {
            return new NewPersonMembership(
                user: user,
                personId: this.PersonId,
                name: this.Name
                );
        }
    }
}