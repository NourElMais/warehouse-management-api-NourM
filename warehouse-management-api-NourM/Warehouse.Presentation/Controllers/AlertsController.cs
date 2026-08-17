using Microsoft.AspNetCore.Mvc;
using Warehouse.Presentation.Alerts;

namespace Warehouse.Presentation.Controllers;

[ApiController]
[Route("api/alerts")]
public class AlertsController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AlertsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }
// endpoint to test that the request is reaching n8n
    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception("Test unhandled exception from Warehouse API");
    }
    [HttpPost("bug")]
    public async Task<IActionResult> ReportBug([FromBody] BugAlertRequest request, CancellationToken cancellationToken)
    {
        string? webhookUrl = _configuration["BugAlertWebhook:Url"];

        if (string.IsNullOrWhiteSpace(webhookUrl))
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Bug alert webhook URL is not configured.");
        }

        HttpClient client = _httpClientFactory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(webhookUrl, request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Failed to forward the bug alert to n8n.");
        }

        return Ok("Bug alert sent successfully.");
    }
}