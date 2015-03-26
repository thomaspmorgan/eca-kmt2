using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    public class ProjectServiceUpdateValidationEntity
    {
        public ProjectServiceUpdateValidationEntity(
            PublishedProject updatedProject, 
            Project projectToUpdate, 
            Focus focus,
            bool goalsExist, 
            bool themesExist, 
            bool pointsOfContactExist)
        {
            this.Name = updatedProject.Name;
            this.Description = updatedProject.Description;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public Focus Focus { get; private set; }

        public bool GoalsExist { get; private set; }

        public bool ThemesExist { get; private set; }

        public bool PointsOfContactExist { get; private set; }

    }
}
