using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NetGuardAI.App.Services;

public class JwtValidationService
{
    private readonly TokenValidationParameters _validationParams;
    private readonly JwtSecurityTokenHandler _handler;
    
    public JwtValidationService(IConfiguration configuration)
    {
        var secret = configuration.GetValue<string>("JwtConfiguration:SecretKey");
        
        _handler = new JwtSecurityTokenHandler();
        _validationParams = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero
        };
    }
    
    public ClaimsPrincipal ValidateToken(string jwt, out SecurityToken validatedToken)
    {
        return _handler.ValidateToken(jwt, _validationParams, out validatedToken);
    }
}