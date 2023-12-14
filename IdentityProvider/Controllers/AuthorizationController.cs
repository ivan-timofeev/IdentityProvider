using System.Security.Cryptography;
using System.Text;
using IdentityProvider.Data;
using IdentityProvider.Models.DataTransferObjects;
using IdentityProvider.Models.DomainModels;
using IdentityProvider.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityProvider.Controllers;

[ApiController]
[Route("api")]
public class AuthorizationController : ControllerBase
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IdentityDbContext _dbContext;

    public AuthorizationController(
        IJwtTokenGenerator jwtTokenGenerator,
        IdentityDbContext dbContext)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _dbContext = dbContext;
    }
    
    [HttpPost("login")]
    public IActionResult Login(
        [FromBody, BindRequired] LoginRequestDto loginRequestDto)
    {
        var user = _dbContext
            .Users
            .Where(u => u.Login == loginRequestDto.Login)
            .SingleOrDefault();

        if (user is null)
            return NotFound("User not found.");

        var passwordHash = Sha256(loginRequestDto.Password);

        if (passwordHash != user.PasswordHash)
            return StatusCode(StatusCodes.Status401Unauthorized, "Wrong password");

        var jwtToken = _jwtTokenGenerator.GenerateToken(Guid.NewGuid());

        return Ok(jwtToken);
    }
    
    [HttpPost("register")]
    public IActionResult Register(
        [FromBody, BindRequired] RegisterRequestDto registerRequestDto)
    {
        var user = _dbContext
            .Users
            .Where(u => u.Login == registerRequestDto.Login)
            .SingleOrDefault();

        if (user is not null)
            return BadRequest("User with provided login already exists.");

        _dbContext.Users.Add(
            new User
            {
                Login = registerRequestDto.Login,
                PasswordHash = Sha256(registerRequestDto.Password)
            });

        _dbContext.SaveChanges();

        return Ok();
    }

    [Authorize]
    [HttpGet("/check")]
    public IActionResult CheckAuth()
    {
        return Ok();
    }
    
    private static string Sha256(string value)
    {
        var sb = new StringBuilder();

        using (var hash = SHA256.Create())            
        {
            var enc = Encoding.UTF8;
            var result = hash.ComputeHash(enc.GetBytes(value));

            foreach (byte b in result)
                sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
}
