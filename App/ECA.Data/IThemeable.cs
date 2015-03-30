using System;
namespace ECA.Data
{
    /// <summary>
    /// An IThemeable entity is an entity that has themes.
    /// </summary>
    public interface IThemeable
    {
        /// <summary>
        /// Gets or sets the themes.
        /// </summary>
        System.Collections.Generic.ICollection<Theme> Themes { get; set; }
    }
}
