using Kolokwium.Models;
using Microsoft.Data.SqlClient;
using Task = Kolokwium.Models.Task;

namespace Kolokwium.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IConfiguration _configuration;

        public TaskRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Task> GetTasks(int? projectId)
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
            connection.Open();

            var query = "SELECT * FROM Task";
            if (projectId.HasValue)
            {
                query += " WHERE IdProject = @IdProject";
            }

            using var command = new SqlCommand(query, connection);
            if (projectId.HasValue)
            {
                command.Parameters.AddWithValue("@IdProject", projectId.Value);
            }

            using var reader = command.ExecuteReader();

            var tasks = new List<Task>();
            while (reader.Read())
            {
                tasks.Add(new Task
                {
                    IdTask = (int)reader["IdTask"],
                    Name = reader["Name"].ToString()!,
                    Description = reader["Description"].ToString()!,
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    IdProject = (int)reader["IdProject"],
                    IdReporter = (int)reader["IdReporter"],
                    IdAssignee = (int)reader["IdAssignee"]
                });
            }

            return tasks;
        }

        public void AddTask(Task task)
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
            connection.Open();

            var command = new SqlCommand(
                "INSERT INTO Task (Name, Description, CreatedAt, IdProject, IdReporter, IdAssignee) " +
                "VALUES (@Name, @Description, @CreatedAt, @IdProject, @IdReporter, @IdAssignee)", connection);

            command.Parameters.AddWithValue("@Name", task.Name);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@CreatedAt", task.CreatedAt);
            command.Parameters.AddWithValue("@IdProject", task.IdProject);
            command.Parameters.AddWithValue("@IdReporter", task.IdReporter);
            command.Parameters.AddWithValue("@IdAssignee", task.IdAssignee);

            command.ExecuteNonQuery();
        }
    }

    public interface ITaskRepository
    {
        IEnumerable<Task> GetTasks(int? projectId);
        void AddTask(Task task);
    }
}
