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
    /// Binding model for editing pii
    /// </summary>
    public class GeneralBindingModel
    {
        /// <summary>
        /// Gets and sets the person id
        /// </summary>
        [Required]
        public int PersonId { get; set; }

        public List<int> ProminentCategories { get; set; }

        /// <summary>
        /// Convert binding model to business model 
        /// </summary>
        /// <param name="user">The user updating the pii</param>
        /// <returns>Update pii business model</returns>
        public UpdateGeneral ToUpdateGeneral(User user)
        {
            return new UpdateGeneral(
                updatedBy: user,
                personId: this.PersonId,
                prominentCategories: this.ProminentCategories
                );
        }
    }
}