using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Projects
{
    /// <summary>
    /// A ProjectServiceCreateValidationEntity is used to validate a new Project.
    /// </summary>
    public class ProjectServiceCreateValidationEntity
    {
        /// <summary>
        /// Creates a new ProjectserviceCreateValidationEntity.
        /// </summary>
        /// <param name="name">The name of the project.</param>
        /// <param name="description">The description of the project.</param>
        /// <param name="program">The program the project belongs to.</param>
        public ProjectServiceCreateValidationEntity(string name, string description, Program program)
        {
            this.Program = program;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Gets the program this project belongs to.
        /// </summary>
        public Program Program { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description { get; private set; }
    }
}
