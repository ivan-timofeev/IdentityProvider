namespace IdentityProvider.Models.DataTransferObjects;

public class LoginRequestDto
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}