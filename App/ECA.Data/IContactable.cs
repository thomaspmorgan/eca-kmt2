using System;
namespace ECA.Data
{
    /// <summary>
    /// An IContactable entity is an entity that has points of contact.
    /// </summary>
    public interface IContactable
    {
        /// <summary>
        /// Gets or sets the points of contact.
        /// </summary>
        System.Collections.Generic.ICollection<Contact> Contacts { get; set; }
    }
}
