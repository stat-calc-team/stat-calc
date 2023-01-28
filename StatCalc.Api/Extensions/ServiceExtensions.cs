using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using StatCalc.Infrastructure.Auth;

namespace StatCalc.Api.Extensions;

public static class ServiceExtensions
{
    public static void ApplySwaggerSettings(this IServiceCollection service)
    {
        string XmlCommentsFilePathContentShare()
        {
            // For PlatformServices used NuGet package Microsoft.Extensions.PlatformAbstractions v1.1.0
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var fileName = typeof(Program).GetTypeInfo().Assembly.GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
        }
            
        service.AddSwaggerGen(c =>
        {
            c.IncludeXmlComments(XmlCommentsFilePathContentShare());
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "StatCalc", Version = "v1"});
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    /// <summary>
    /// Adds authentication for project
    /// </summary>
    /// <param name="service"><see cref="IServiceCollection"/></param>
    public static void AddAuth(this IServiceCollection service)
    {
        service.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer();
        
        // Add jwt bearer options for google
        service.AddTransient<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
    }
    
}
