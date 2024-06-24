using Kolokwium.Models;
using System.Data.SqlClient;
using Kolokwium.Exceptions;
using Microsoft.Data.SqlClient;

namespace Kolokwium.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IConfiguration _configuration;

        public ProjectRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Project GetProjectById(int id)
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
            connection.Open();

            var command = new SqlCommand(
                "SELECT IdProject, Name, IdDefaultAssignee FROM Project WHERE IdProject = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Project
                {
                    IdProject = (int)reader["IdProject"],
                    Name = reader["Name"].ToString()!,
                    IdDefaultAssignee = (int)reader["IdDefaultAssignee"]
                };
            }
            else
            {
                throw new NotFoundException($"Project with id {id} not found");
            }
        }
    }

    public interface IProjectRepository
    {
        Project GetProjectById(int id);
    }
}