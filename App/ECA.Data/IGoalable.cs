using System;
namespace ECA.Data
{
    /// <summary>
    /// An IGoalable entity is an entity that has goals.
    /// </summary>
    public interface IGoalable
    {
        /// <summary>
        /// Gets or sets the goals.
        /// </summary>
        System.Collections.Generic.ICollection<Goal> Goals { get; set; }
    }
}
