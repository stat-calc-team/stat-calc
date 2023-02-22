using StatCalc.Infrastructure.Services;

namespace StatCalc.Api.Extensions;

public static class DIExtensions
{
    public static void AddDI(this IServiceCollection service)
    {
        service.AddTransient<ITestService, TestService>();
        service.AddTransient<IAuthService, AuthService>();
    }
}
