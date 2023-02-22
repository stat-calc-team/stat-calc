using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatCalc.Infrastructure.Dtos;
using StatCalc.Infrastructure.Services;

namespace StatCalc.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    /// <summary>
    /// Returns access token for google authorization
    /// </summary>
    /// <param name="idToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<AuthorizationDto> Authorize([FromQuery] string idToken)
    {
        // TODO add user to database if user does not exist
        return await _authService.GetAuthTokenFromIdToken(idToken);
    }
}
