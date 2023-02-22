namespace StatCalc.Infrastructure.Services;

public class TestService : ITestService
{
    /// <inheritdoc/>
    public async Task<string> HealthCheck()
    {
        return await Task.Run(() => "Feeling good");
    }
}
