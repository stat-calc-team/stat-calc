namespace StatCalc.Infrastructure.Services;

public interface ITestService
{
    /// <summary>
    /// Test action for tests
    /// </summary>
    Task<string> HealthCheck();
}
