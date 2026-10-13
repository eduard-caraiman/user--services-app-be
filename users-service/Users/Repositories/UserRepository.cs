using Microsoft.EntityFrameworkCore;
using users_service.Database;

namespace users_service.Users.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _dbContext;

    public UserRepository(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User[]> GetAllUsersAsync()
    {
        return await _dbContext.Users.ToArrayAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetUserByIdTrackingAsync(Guid userId)
    {
        return await _dbContext.Users
            .AsTracking()
            .SingleOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task DeleteUserAsync(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}