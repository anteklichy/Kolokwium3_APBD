using Kolokwium.Models;
using Kolokwium.Repositories;

namespace Kolokwium.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public Project GetProjectById(int id)
        {
            return _projectRepository.GetProjectById(id);
        }
    }

    public interface IProjectService
    {
        Project GetProjectById(int id);
    }
}