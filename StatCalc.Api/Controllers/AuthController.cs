using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StatCalc.Infrastructure.Auth;
using StatCalc.Infrastructure.Configurations;

namespace StatCalc.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtGenerator _jwtGenerator;
    private readonly GoogleSettings _googleSettings;
    
    public AuthController(IOptions<GoogleSettings> googleSettings)
    {
        _googleSettings = googleSettings.Value;
        _jwtGenerator = new JwtGenerator(_googleSettings.PrivateKey); 
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="idToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("Token")]
    public IActionResult GetToken([FromQuery] string idToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new List<string> { _googleSettings.ClientId }
        };

        var payload  = GoogleJsonWebSignature.ValidateAsync(idToken, settings).Result;
        
        return Ok(new
        {
            AuthToken = _jwtGenerator.CreateUserAuthToken(payload.Email)
        });
    }
}
