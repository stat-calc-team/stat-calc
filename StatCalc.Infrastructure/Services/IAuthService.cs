using StatCalc.Infrastructure.Dtos;

namespace StatCalc.Infrastructure.Services;

public interface IAuthService
{
    /// <summary>
    /// Validates id token and returns access token 
    /// </summary>
    /// <param name="idToken">Received from google</param>
    /// <returns><see cref="AuthorizationDto"/></returns>
    Task<AuthorizationDto> GetAuthTokenFromIdToken(string idToken);
}
