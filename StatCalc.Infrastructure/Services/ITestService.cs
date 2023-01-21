namespace StatCalc.Infrastructure.Services;

public interface ITestService
{
    Task<string> HealthCheck();
}
