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
    public class PersonMembershipBindingModel
    {
        /// <summary>
        /// Gets and sets the person id
        /// </summary>
        [Required]
        public int PersonId { get; set; }

        /// <summary>
        /// Gets and sets the prominent categories for the user
        /// </summary>
        public List<string> Memberships { get; set; }

        /// <summary>
        /// Convert binding model to business model 
        /// </summary>
        /// <param name="user">The user updating the membership</param>
        /// <returns>Update membership business model</returns>
        public UpdateMembership ToUpdateMembership(User user)
        {
            return new UpdateMembership(
                updatedBy: user,
                personId: this.PersonId,
                memberships: this.Memberships
                );
        }
    }
}