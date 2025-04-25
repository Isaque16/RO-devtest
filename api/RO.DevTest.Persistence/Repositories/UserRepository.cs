namespace RO.DevTest.Persistence.Repositories;

using RO.DevTest.Application.Contracts.Persistance.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class UserRepository(DefaultContext context) 
    : BaseRepository<User>(context), IUserRepository
{
    private readonly DefaultContext _context = context;

    public async Task<User?> FindUserByUserNameAsync(string username)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentException($"{nameof(username)} cannot be null or empty", nameof(username));

        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<IList<string>> GetUserRolesAsync(User user)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
            .Where(name => name != null)
            .Cast<string>()
            .ToListAsync();
    }

    public async Task<bool> PasswordSignInAsync(User user, string password)
    {
        var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        if (dbUser == null)
            return false;
        
        // Verificação provisória de senha
        return dbUser.Password == password;
    }
}
