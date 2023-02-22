using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StatCalc.Infrastructure.Configurations;

namespace StatCalc.Infrastructure.Auth;

public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly GoogleSettings _googleSettings;
    
    public ConfigureJwtBearerOptions(IOptions<GoogleSettings> googleSettings)
    {
        _googleSettings = googleSettings.Value;
    }
    
    public void Configure(string name, JwtBearerOptions options)
    {
        var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(_googleSettings.PublicKey), out _);

        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ValidateIssuer = true,
            ValidIssuer = "AuthService",
            ValidateAudience = true,
            ValidAudience = "StatCalcApi",
            CryptoProviderFactory = new CryptoProviderFactory()
            {
                CacheSignatureProviders = false
            }
        };
    }

    public void Configure(JwtBearerOptions options)
    {
        throw new NotImplementedException();
    }
}
