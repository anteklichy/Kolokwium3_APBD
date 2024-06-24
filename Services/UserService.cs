using Kolokwium.Models;
using Kolokwium.Repositories;

namespace Kolokwium.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }
    }

    public interface IUserService
    {
        User GetUserById(int id);
    }
}