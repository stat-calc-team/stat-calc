using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatCalc.Infrastructure.Services;

namespace StatCalc.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ITestService _testService;

    public TestController(ITestService testService)
    {
        _testService = testService;
    }

    /// <summary>
    /// Just simple health check endpoint
    /// </summary>
    [HttpGet]
    [Route("[action]")]
    [Authorize]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<string> HealthCheck()
    {
        return await _testService.HealthCheck();
    }
}
