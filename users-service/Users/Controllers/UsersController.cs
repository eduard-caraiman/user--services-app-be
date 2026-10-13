using Microsoft.AspNetCore.Mvc;
using users_service.Filters;
using users_service.Users.Requests;
using users_service.Users.Responses;
using users_service.Users.Service;

namespace users_service.Users.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(
        IUserService userService)
    {
        _userService = userService;
    }


    /// <summary>
    /// Gets all Users
    /// </summary>
    /// <returns>Return the users in JSON array</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users.Select(user => GetUserResponse.From(user)));
    }

    /// <summary>
    /// Get User by ID
    /// </summary>
    /// <returns>Return the User in JSON</returns>
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(GetUserResponse.From(user));
    }


    /// <summary>
    /// Create User
    /// </summary>
    /// <returns>Return the new created User</returns>
    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<CreateUserRequest>))]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var newUser = await _userService.CreateUserAsync(request);

        return Created($"/api/users/{newUser.Id}", GetUserResponse.From(newUser));
    }


    /// <summary>
    /// Update User
    /// </summary>
    /// <returns>Return the updated User</returns>
    [HttpPut("{userId}")]
    [ServiceFilter(typeof(ValidationFilter<UpdateUserRequest>))]
    [ProducesResponseType(typeof(GetUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] UpdateUserRequest request)
    {
        var newUser = await _userService.UpdateUserAsync(userId, request);

        if (newUser is null)
        {
            return NotFound();
        }

        return Ok(GetUserResponse.From(newUser));
    }

    /// <summary>
    /// Delete User
    /// </summary>
    /// <returns>Return no content</returns>
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
    {
        var deletedUser = await _userService.DeleteUserAsync(userId);

        if (deletedUser == false)
        {
            return NotFound();
        }

        return NoContent();
    }
}