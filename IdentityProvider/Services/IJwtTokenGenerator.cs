namespace IdentityProvider.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId);
}
