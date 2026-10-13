using users_service.Users.Repositories;
using users_service.Users.Requests;

namespace users_service.Users.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }


    public async Task<User[]> GetAllUsersAsync()
    {
        _logger.LogInformation("Getting all users");

        var users = await _userRepository.GetAllUsersAsync();

        _logger.LogInformation("Retrieved {UserCount} users successfully.", users.Length);

        return users;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        _logger.LogInformation("Getting user with ID {UserId}", userId);

        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user is null)
        {
            _logger.LogWarning("User with ID {UserId} was not found!", userId);
            return null;
        }

        _logger.LogInformation("User with ID {UserId} was found.", userId);

        return user;
    }

    public async Task<User> CreateUserAsync(CreateUserRequest request)
    {
        _logger.LogInformation("Starting create a new user.");


        var currentDate = DateTime.UtcNow;
        var newUser = request.To();

        newUser.CreatedAt = currentDate;
        newUser.UpdatedAt = currentDate;


        var createdUser = await _userRepository.CreateUserAsync(newUser);

        _logger.LogInformation("The User with ID {UserId} was created successfully.", createdUser.Id);

        return createdUser;
    }

    public async Task<User?> UpdateUserAsync(Guid userId, UpdateUserRequest request)
    {
        _logger.LogInformation("Starting updating user with ID {UserId}.", userId);
        var currentUser = await _userRepository.GetUserByIdTrackingAsync(userId);
        if (currentUser is null)
        {
            _logger.LogWarning("User with ID {UserId} was not found!", userId);
            return null;
        }

        request.To(currentUser);
        currentUser.UpdatedAt = DateTime.UtcNow;

        await _userRepository.SaveChangesAsync();

        _logger.LogInformation("The User with ID {UserId} was updated successfully.", userId);

        return currentUser;
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        _logger.LogInformation("Starting deleting user with ID {UserId}.", userId);

        var user = await _userRepository.GetUserByIdTrackingAsync(userId);

        if (user is null)
        {
            _logger.LogWarning("User with ID {UserId} was not found.", userId);
            return false;
        }

        await _userRepository.DeleteUserAsync(user);

        _logger.LogInformation("The User with ID {UserId} was deleted successfully.", userId);
        return true;
    }
}