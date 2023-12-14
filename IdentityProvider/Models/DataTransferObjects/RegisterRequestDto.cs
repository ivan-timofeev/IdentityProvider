namespace IdentityProvider.Models.DataTransferObjects;

public class RegisterRequestDto
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}
