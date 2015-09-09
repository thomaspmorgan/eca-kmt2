using ECA.Business.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ECA.Business.Service.Persons;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for editing contact Info
    /// </summary>
    public class ContactInfoBindingModel
    {
        /// <summary>
        /// Gets and sets the person id
        /// </summary>
        [Required]
        public int PersonId { get; set; }

        /// <summary>
        /// Gets and sets the prominent categories for the user
        /// </summary>
        public bool HasContactAgreement { get; set; }

        /// <summary>
        /// Convert binding model to business model 
        /// </summary>
        /// <param name="user">The user updating the contactInfo</param>
        /// <returns>Update contactInfo business model</returns>
        public UpdateContactInfo ToUpdateContactInfo(User user)
        {
            return new UpdateContactInfo(
                updatedBy: user,
                personId: this.PersonId,
                hasContactAgreement: this.HasContactAgreement
                );
        }
    }
}