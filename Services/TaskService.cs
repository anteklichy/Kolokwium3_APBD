using Kolokwium.Exceptions;
using Kolokwium.Repositories;
using Task = Kolokwium.Models.Task;

namespace Kolokwium.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccessRepository _accessRepository;

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository, IAccessRepository accessRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _accessRepository = accessRepository;
        }

        public IEnumerable<Task> GetTasks(int? projectId)
        {
            return _taskRepository.GetTasks(projectId);
        }

        public void AddTask(Task task)
        {
            var reporter = _userRepository.GetUserById(task.IdReporter) 
                           ?? throw new NotFoundException($"Reporter with id {task.IdReporter} not found");

            var assignee = _userRepository.GetUserById(task.IdAssignee)
                            ?? throw new NotFoundException($"User with id {task.IdAssignee} not found");

            if (assignee == null)
            {
                throw new NotFoundException($"Assignee with id {task.IdAssignee} or default assignee not found");
            }

            if (!_accessRepository.UserHasAccessToProject(reporter.IdUser, task.IdProject) ||
                !_accessRepository.UserHasAccessToProject(assignee.IdUser, task.IdProject))
            {
                throw new NotFoundException("Reporter or assignee does not have access to the project");
            }

            task.CreatedAt = DateTime.Now;
            task.IdAssignee = assignee.IdUser;

            _taskRepository.AddTask(task);
        }
    }

    public interface ITaskService
    {
        IEnumerable<Task> GetTasks(int? projectId);
        void AddTask(Task task);
    }
}
