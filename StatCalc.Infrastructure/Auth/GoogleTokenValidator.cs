using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace StatCalc.Infrastructure.Auth;

public class GoogleTokenValidator : ISecurityTokenValidator
{
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public GoogleTokenValidator()
    {
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public bool CanValidateToken => true;

    public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

    public bool CanReadToken(string securityToken)
    {
        return _tokenHandler.CanReadToken(securityToken);
    }

    public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        validatedToken = null;
        var payload = GoogleJsonWebSignature.ValidateAsync(securityToken, new GoogleJsonWebSignature.ValidationSettings()).Result; // here is where I delegate to Google to validate

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, payload.Name),
            new Claim(ClaimTypes.Name, payload.Name),
            new Claim(JwtRegisteredClaimNames.FamilyName, payload.FamilyName),
            new Claim(JwtRegisteredClaimNames.GivenName, payload.GivenName),
            new Claim(JwtRegisteredClaimNames.Email, payload.Email),
            new Claim(JwtRegisteredClaimNames.Sub, payload.Subject),
            new Claim(JwtRegisteredClaimNames.Iss, payload.Issuer),
        };

        try
        {
            var principle = new ClaimsPrincipal();
            principle.AddIdentity(new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme));
            return principle;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;

        }
    }
}

public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(string name, JwtBearerOptions options)
    {
        RSA rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(PUBLIC_KEY), out _);

        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(rsa),
            ValidateIssuer = true,
            ValidIssuer = "AuthService",
            ValidateAudience = true,
            ValidAudience = "myApi",
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
    private const string PUBLIC_KEY = @"MIICCgKCAgEAtCR2Pii+q9C76P2E9ydHYxnBPjJFGT7MvHuQPKpcS9RImfrkobt0
        LPS/406eWm/tRBvnYD9nDpHJNKN3TjEenFQuDGR4RHcGK/e43SAhTAi7+s0tfAQd
        6BK4gznIwvs5cWyilh1B7c9sCnxhJ/EYLIe1N2yiD8mhvfojIF4vMYxONIMTGYXy
        87lnO9zRAdXAZ39YbtmFmQwK8gfXX5d/XVlKy0tc2y5bRY5iXn9kwqwvFlzL6O4v
        pjhqA5kwsJV7efhL9nU0ACR4dG3zwFR3SAOOSETXjnfmjH2ocga+oa65ToypUz2L
        1DwnNHt+M5CtDJ9um4dbYaqfBWkjWe3FuGB0GNPS8pbX2nVt76OfHA/QKmxTWvFd
        POZnjpg2QhDujyXgoIY731zx5bAklKVoKFma/qfWfCyCSTUzhgu1KQm9swipMsQy
        NYr9CjbnIlPn4EvrBIbGcIiaRNCLCIlcAuxE/GiH1zBUfeJxfJQmurejp6mBAtAS
        FY08DmUebBz8mlUbB+LXMYKHZ4GK6TecPy0WJU2qRMQ//PKfOa+wkesp4M53SQdp
        ItDp5akTzYUo4rXwk3HPCtemKaSNhyG+EYtZ1CAmPN5sEjU0/x0Dq7SU5o8KhogB
        m/5HRJ3M9dMRcwD3OcsMl0kW1PPUt04itboS3SlFav90V9uc2YNGpPsCAwEAAQ==";
}