using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Allows a business layer client to add a Membership to a person.
    /// </summary>
    public class PersonMembership
    {
        /// <summary>
        /// Creates a new membership with the user, and name.
        /// </summary>
        /// <param name="user">The user creating the membership.</param>
        /// <param name="name">The name of the membership.</param>
        /// <param name="personId">The person id.</param>
        public PersonMembership(User user, string name, int personId)
        {
            this.PersonId = personId;
            this.Name = name;
            this.Create = new Create(user);
        }

        /// <summary>
        /// Gets/sets the person id.
        /// </summary>
        public int PersonId { get; private set; }
        
        /// <summary>
        /// Gets/sets the name of the Membership
        /// </summary>
        public string Name {get; set;}

        /// <summary>
        /// Gets/sets the create audit info.
        /// </summary>
        public Create Create { get; private set; }

        /// <summary>
        /// Adds the given 
        /// </summary>
        /// <param name="socialable">The socialable entity.</param>
        /// <returns>The membership that should be added to the context.</returns>
        public Membership AddPersonMembership(Person person)
        {
            Contract.Requires(person != null, "The membership entity must not be null.");
            var membership = new Membership
            {
                Name = this.Name
            };
            this.Create.SetHistory(membership);
            person.Memberships.Add(membership);
            return membership;
        }
    }
}