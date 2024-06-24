using Kolokwium.Models;
using Kolokwium.Exceptions;
using Microsoft.Data.SqlClient;

namespace Kolokwium.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public User GetUserById(int id)
        {
            using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
            connection.Open();

            var command = new SqlCommand(
                "SELECT IdUser, FirstName, LastName FROM [User] WHERE IdUser = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    IdUser = (int)reader["IdUser"],
                    FirstName = reader["FirstName"].ToString()!,
                    LastName = reader["LastName"].ToString()!
                };
            }
            else
            {
                throw new NotFoundException($"User with id {id} not found");
            }
        }
    }

    public interface IUserRepository
    {
        User GetUserById(int id);
    }
}