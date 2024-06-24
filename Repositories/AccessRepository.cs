using Kolokwium.Models;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Kolokwium.Repositories
{
    public class AccessRepository : IAccessRepository
    {
        private readonly IConfiguration _configuration;

        public AccessRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool UserHasAccessToProject(int userId, int projectId)
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
            connection.Open();

            var command = new SqlCommand(
                "SELECT COUNT(*) FROM Access WHERE IdUser = @UserId AND IdProject = @ProjectId", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@ProjectId", projectId);

            var count = (int)command.ExecuteScalar();
            return count > 0;
        }
    }

    public interface IAccessRepository
    {
        bool UserHasAccessToProject(int userId, int projectId);
    }
}