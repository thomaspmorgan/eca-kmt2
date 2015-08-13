using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
namespace ECA.Business.Service.Projects
{
    /// <summary>
    /// A ProjectService is a service capable of performing crud on projects.
    /// </summary>
    [ContractClass(typeof(ProjectServiceContract))]
    public interface IProjectService : ISaveable
    {
        /// <summary>
        /// Creates and returns project
        /// </summary>
        /// <param name="project">The project to create</param>
        /// <returns>The project that was created</returns>
        Project Create(DraftProject project);

        /// <summary>
        /// Creates and returns project asyncronously
        /// </summary>
        /// <param name="project">The project to create</param>
        /// <returns>The project that was created</returns>
        Task<Project> CreateAsync(DraftProject project);

        /// <summary>
        /// Returns the sorted, filtered, and paged projects in the program with the given program id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted projects.</returns>
        PagedQueryResults<SimpleProjectDTO> GetProjectsByProgramId(int programId, QueryableOperator<SimpleProjectDTO> queryOperator);

        /// <summary>
        /// Returns the sorted, filtered, and paged projects in the program with the given program id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted projects.</returns>
        Task<PagedQueryResults<SimpleProjectDTO>> GetProjectsByProgramIdAsync(int programId, QueryableOperator<SimpleProjectDTO> queryOperator);

        /// <summary>
        /// Returns projects by person id
        /// </summary>
        /// <param name="personId">The person id</param>
        /// <param name="queryOperator">The query operator</param>
        /// <returns>Projects by person id</returns>
        PagedQueryResults<ParticipantTimelineDTO> GetProjectsByPersonId(int personId, QueryableOperator<ParticipantTimelineDTO> queryOperator);

        /// <summary>
        /// Returns projects by person id
        /// </summary>
        /// <param name="personId">The person id</param>
        /// <param name="queryOperator">The query operator</param>
        /// <returns>Projects by person id</returns>
        Task<PagedQueryResults<ParticipantTimelineDTO>> GetProjectsByPersonIdAsync(int personId, QueryableOperator<ParticipantTimelineDTO> queryOperator);

        /// <summary>
        /// Returns all projects, sorted, filtered, and paged within the eca system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted projects.</returns>
        PagedQueryResults<SimpleProjectDTO> GetProjects(QueryableOperator<SimpleProjectDTO> queryOperator);

        /// <summary>
        /// Returns all projects, sorted, filtered, and paged within the eca system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted projects.</returns>
        Task<PagedQueryResults<SimpleProjectDTO>> GetProjectsAsync(QueryableOperator<SimpleProjectDTO> queryOperator);

        /// <summary>
        /// Returns a project by id asynchronously
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns>Project</returns>
        Task<ProjectDTO> GetProjectByIdAsync(int projectId);

        /// <summary>
        /// Returns a project by id  
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <returns>Project</returns>
        ProjectDTO GetProjectById(int projectId);

        /// <summary>
        /// Updates the project in the system with the given project.
        /// </summary>
        /// <param name="updatedProject">The updated project.</param>
        void Update(PublishedProject updatedProject);

        /// <summary>
        /// Updates the project in the system with the given project.
        /// </summary>
        /// <param name="updatedProject">The updated project.</param>
        Task UpdateAsync(PublishedProject updatedProject);

        /// <summary>
        /// Adds the participant to the project.
        /// </summary>
        /// <param name="additionalParticipant">The additional participant.</param>
        void AddParticipant(AdditionalProjectParticipant additionalParticipant);

        /// <summary>
        /// Adds the participant to the project.
        /// </summary>
        /// <param name="additionalParticipant">The additional participant.</param>
        Task AddParticipantAsync(AdditionalProjectParticipant additionalParticipant);

        /// <summary>
        /// Adds a new project location to the system.
        /// </summary>
        /// <param name="projectLocation">The location to add to the project.</param>
        /// <returns>The location entity added to the datastore.</returns>
        Location CreateLocation(AdditionalProjectLocation projectLocation);

        /// <summary>
        /// Adds a new project location to the system.
        /// </summary>
        /// <param name="projectLocation">The location to add to the project.</param>
        /// <returns>The location entity added to the datastore.</returns>
        Task<Location> CreateLocationAsync(AdditionalProjectLocation projectLocation);

        /// <summary>
        /// Updates the project's location with the updated values.
        /// </summary>
        /// <param name="projectLocation">The updated project location details.</param>
        void UpdateLocation(UpdatedProjectLocation projectLocation);

        /// <summary>
        /// Updates the project's location with the updated values.
        /// </summary>
        /// <param name="projectLocation">The updated project location details.</param>
        /// <returns>The task.</returns>
        Task UpdateLocationAsync(UpdatedProjectLocation projectLocation);
    }

    /// <summary>
    /// 
    /// </summary>
    [ContractClassFor(typeof(IProjectService))]
    public abstract class ProjectServiceContract : IProjectService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public Project Create(DraftProject project)
        {
            Contract.Requires(project != null, "The project must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public Task<Project> CreateAsync(DraftProject project)
        {
            Contract.Requires(project != null, "The project must not be null.");
            return Task.FromResult<Project>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<SimpleProjectDTO> GetProjectsByProgramId(int programId, QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<SimpleProjectDTO>> GetProjectsByProgramIdAsync(int programId, QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<SimpleProjectDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<ParticipantTimelineDTO> GetProjectsByPersonId(int personId, QueryableOperator<ParticipantTimelineDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<ParticipantTimelineDTO>> GetProjectsByPersonIdAsync(int personId, QueryableOperator<ParticipantTimelineDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<ParticipantTimelineDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Task<ProjectDTO> GetProjectByIdAsync(int projectId)
        {
            return Task.FromResult<ProjectDTO>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ProjectDTO GetProjectById(int projectId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedProject"></param>
        public void Update(PublishedProject updatedProject)
        {
            Contract.Requires(updatedProject != null, "The updated project must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedProject"></param>
        /// <returns></returns>
        public Task UpdateAsync(PublishedProject updatedProject)
        {
            Contract.Requires(updatedProject != null, "The updated project must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult<int>(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="additionalParticipant"></param>
        public void AddParticipant(AdditionalProjectParticipant additionalParticipant)
        {
            Contract.Requires(additionalParticipant != null, "The additional participant must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="additionalParticipant"></param>
        /// <returns></returns>
        public Task AddParticipantAsync(AdditionalProjectParticipant additionalParticipant)
        {
            Contract.Requires(additionalParticipant != null, "The additional participant must not be null.");
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public PagedQueryResults<SimpleProjectDTO> GetProjects(QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryOperator"></param>
        /// <returns></returns>
        public Task<PagedQueryResults<SimpleProjectDTO>> GetProjectsAsync(QueryableOperator<SimpleProjectDTO> queryOperator)
        {
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            return Task.FromResult<PagedQueryResults<SimpleProjectDTO>>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectLocation"></param>
        /// <returns></returns>
        public Location CreateLocation(AdditionalProjectLocation projectLocation)
        {
            Contract.Requires(projectLocation != null, "The project location must not be null.");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectLocation"></param>
        /// <returns></returns>
        public Task<Location> CreateLocationAsync(AdditionalProjectLocation projectLocation)
        {
            Contract.Requires(projectLocation != null, "The project location must not be null.");
            return Task.FromResult<Location>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectLocation"></param>
        public void UpdateLocation(UpdatedProjectLocation projectLocation)
        {
            Contract.Requires(projectLocation != null, "The project location must not be null.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectLocation"></param>
        /// <returns></returns>
        public Task UpdateLocationAsync(UpdatedProjectLocation projectLocation)
        {
            Contract.Requires(projectLocation != null, "The project location must not be null.");
            return Task.FromResult<Object>(null);
        }
    }
}
