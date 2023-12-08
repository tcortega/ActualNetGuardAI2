using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetGuardAI.App.Services;
using NetGuardAI.Core.Persistence;

namespace NetGuardAI.App.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly JwtValidationService _jwtValidator;
    private readonly AuthenticationState _anonymousUser = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public CustomAuthenticationStateProvider(ILocalStorageService localStorage, IConfiguration configuration,
        AppDbContext context, JwtValidationService jwtValidator)
    {
        _localStorage = localStorage;
        _configuration = configuration;
        _context = context;
        _jwtValidator = jwtValidator;
    }

    /// <inheritdoc/>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var jwt = await _localStorage.GetItemAsync<string>("jwt");

            var authenticatedUser = _jwtValidator.ValidateToken(jwt, out _);
            return new AuthenticationState(authenticatedUser);
        }
        catch
        {
            // Token not present, expired or forged
            return _anonymousUser;
        }
    }

    public async Task AuthenticateUser(string username, string password)
    {
        // ReSharper disable once SpecifyStringComparison
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower());
        if (user is null) throw new InvalidCredentialException("Could not find an user with the given username");

        if (user.Password != password) throw new InvalidCredentialException("Invalid password");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            // new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var jwt = BuildJwtToken(claims);
        await _localStorage.SetItemAsync("jwt", jwt);

        var identity = new ClaimsIdentity(claims, "Admin");
        var admin = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(admin)));
    }

    public async Task<int> GetCurrentUserId()
    {
        var user = await GetAuthenticationStateAsync();
        var claims = user.User.Claims;
        var username = claims.First(c => c.Type == ClaimTypes.Name).Value;
        
        var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        return userEntity?.Id ?? -1;
    }

    private string BuildJwtToken(IEnumerable<Claim> claims)
    {
        var secret = _configuration.GetValue<string>("JwtConfiguration:SecretKey")!;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var hours = Math.Clamp(24, 0, 9999);
        var expiration = DateTime.UtcNow.AddHours(hours);

        var token = new JwtSecurityToken(null, null, claims, DateTime.UtcNow, expiration, creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task Logout() => await _localStorage.RemoveItemAsync("jwt");
}