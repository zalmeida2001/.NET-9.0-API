using TodoListApi.Models;
using System.Threading.Tasks;

namespace TodoListApi.Repositories
{
    public interface IAuthRepository
    {
        Task<User> GetUserByUsername(string username);
        Task<bool> Register(User user);
    }
}
