namespace users_service.Users.Responses;

public class GetUserResponse
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static GetUserResponse From(User user)
    {
        return new GetUserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
        };
    }
}