using ECA.Business.Service.Admin;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The PersonEmailAddressBindingModel is used when a client is creating or updated an email address on a person.
    /// </summary>
    public class PersonEmailAddressBindingModel : EmailAddressBindingModelBase<ECA.Data.Person>
    {
        /// <summary>
        /// Returns a business layer entity to create or update email addreses.
        /// </summary>
        /// <param name="user">The user performing the operation.</param>
        /// <returns>The email address.</returns>
        public override NewEmailAddress<ECA.Data.Person> ToEmailAddress(Business.Service.User user)
        {
            return new NewPersonEmailAddress(user, this.EmailAddressTypeId, this.Address, this.IsPrimary, this.EMailAddressableId);
        }
    }
}