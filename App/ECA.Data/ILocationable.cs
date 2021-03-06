﻿using System;
namespace ECA.Data
{
    /// <summary>
    /// An ILocationable entity is an entity that has locations.
    /// </summary>
    public interface ILocationable
    {
        /// <summary>
        /// Gets or sets the locations.
        /// </summary>
        System.Collections.Generic.ICollection<Location> Locations { get; set; }
    }

    public interface IRegionable
    {
        /// <summary>
        /// Gets or sets the locations.
        /// </summary>
        System.Collections.Generic.ICollection<Location> Regions { get; set; }
    }
}
