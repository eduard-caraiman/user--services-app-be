namespace users_service.Users.Repositories;

public interface IUserRepository
{
    Task<User[]> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByIdTrackingAsync(Guid userId);
    Task<User> CreateUserAsync(User user);
    Task DeleteUserAsync(User user);
    Task SaveChangesAsync();

}