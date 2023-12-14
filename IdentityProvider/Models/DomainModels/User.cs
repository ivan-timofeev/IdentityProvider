namespace IdentityProvider.Models.DomainModels;

public class User
{
    public Guid Id { get; set; }
    public required string Login { get; set; }
    public required string PasswordHash { get; set; }
}
