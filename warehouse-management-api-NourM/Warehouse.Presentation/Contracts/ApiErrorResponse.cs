namespace Warehouse.Presentation.Contracts;

public class ApiErrorResponse
{
    // ErrorCode: a simple code like NOT_FOUND
    // Message: Message with minimal details, to be displayed to the client
    // TraceId: To match the frontend error with backend logs
    public string ErrorCode { get; set; }
    public string Message { get; set; }
    public string TraceId { get; set; }
}