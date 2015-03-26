using System;
namespace ECA.Data
{
    public interface IContactable
    {
        System.Collections.Generic.ICollection<Contact> Contacts { get; set; }
    }
}
