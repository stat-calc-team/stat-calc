namespace StatCalc.Infrastructure.Services;

public class TestService : ITestService
{
    public TestService()
    {
    }

    public async Task<string> HealthCheck()
    {
        return await Task.Run(() => "Feeling good");
    }
}
