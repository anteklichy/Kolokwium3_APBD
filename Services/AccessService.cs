using Kolokwium.Repositories;

namespace Kolokwium.Services
{
    public class AccessService : IAccessService
    {
        private readonly IAccessRepository _accessRepository;

        public AccessService(IAccessRepository accessRepository)
        {
            _accessRepository = accessRepository;
        }

        public bool UserHasAccessToProject(int userId, int projectId)
        {
            return _accessRepository.UserHasAccessToProject(userId, projectId);
        }
    }

    public interface IAccessService
    {
        bool UserHasAccessToProject(int userId, int projectId);
    }
}