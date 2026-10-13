using users_service.Users.Requests;

namespace users_service.Users.Service;

public interface IUserService
{
    Task<User[]> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User> CreateUserAsync(CreateUserRequest user);
    Task<User?> UpdateUserAsync(Guid userId,UpdateUserRequest user);
    Task<bool> DeleteUserAsync(Guid userId);
}