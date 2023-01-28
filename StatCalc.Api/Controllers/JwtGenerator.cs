using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace StatCalc.Api.Controllers;


public class JwtGenerator
{
    readonly RsaSecurityKey _key;
    public JwtGenerator(string privateKey)
    {
        RSA privateRSA = RSA.Create();
        privateRSA.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
        _key = new RsaSecurityKey(privateRSA);
    }

    public string CreateUserAuthToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = "myApi",
            Issuer = "AuthService",
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Sid, userId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}