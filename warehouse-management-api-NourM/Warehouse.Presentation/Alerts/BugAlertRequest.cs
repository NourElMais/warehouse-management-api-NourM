using System.ComponentModel.DataAnnotations;

namespace Warehouse.Presentation.Alerts;

//This DTO represents the JSON that will be sent to the endpoint that receives the alert
public class BugAlertRequest
{
    [Required]
    public string Message { get; set; }

    [Required]
    public string ExceptionType { get; set; }

    [Required]
    public string Endpoint { get; set; } 

    [Required]
    public string HttpMethod { get; set; } 

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}