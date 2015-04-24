using System;
namespace ECA.Data
{
    /// <summary>
    /// An IObjectivable entity is an entity that has objectives.
    /// </summary>
    public interface IObjectivable
    {
        System.Collections.Generic.ICollection<Objective> Objectives { get; set; }
    }
}
