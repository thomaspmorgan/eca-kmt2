using System;
namespace ECA.Data
{
    public interface IThemeable
    {
        System.Collections.Generic.ICollection<Theme> Themes { get; set; }
    }
}
