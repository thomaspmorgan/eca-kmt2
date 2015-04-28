using System;
namespace ECA.Data
{
    /// <summary>
    /// An ICategorizable entity is an entity that has themes.
    /// </summary>
    public interface ICategorizable
    {
        /// <summary>
        /// Gets or sets the themes.
        /// </summary>
        System.Collections.Generic.ICollection<Category> Categories { get; set; }
    }
}
