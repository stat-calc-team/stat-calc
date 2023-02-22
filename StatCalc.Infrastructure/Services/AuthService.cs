using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using StatCalc.Infrastructure.Auth;
using StatCalc.Infrastructure.Configurations;
using StatCalc.Infrastructure.Dtos;

namespace StatCalc.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly JwtGenerator _jwtGenerator;
    private readonly GoogleSettings _googleSettings;
    
    public AuthService(IOptions<GoogleSettings> googleSettings)
    {
        _googleSettings = googleSettings.Value;
        _jwtGenerator = new JwtGenerator(_googleSettings.PrivateKey); 
    }

    /// <inheritdoc/>
    public async Task<AuthorizationDto> GetAuthTokenFromIdToken(string idToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> { _googleSettings.ClientId }
        };

        var payload = GoogleJsonWebSignature.ValidateAsync(idToken, settings).Result;

        var authResponse = new AuthorizationDto()
        {
            AccessToken = _jwtGenerator.CreateUserAuthToken(payload.Email)
        };

        return authResponse;
    }
}
