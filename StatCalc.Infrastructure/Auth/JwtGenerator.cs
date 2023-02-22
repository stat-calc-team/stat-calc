using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace StatCalc.Infrastructure.Auth;

public class JwtGenerator
{
    private readonly RsaSecurityKey _key;
    public JwtGenerator(string privateKey)
    {
        var privateRsa = RSA.Create();
        privateRsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
        _key = new RsaSecurityKey(privateRsa);
    }

    public string CreateUserAuthToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = "StatCalcApi",
            Issuer = "AuthService",
            Subject = new ClaimsIdentity(new []
            {
                new Claim(ClaimTypes.Sid, userId)
            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
