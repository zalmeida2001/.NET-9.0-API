using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TodoListApi.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly TodoContext _context;

        public AuthRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> Register(User user)
        {
            user.PasswordHash = HashPassword(user.PasswordHash);
            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
