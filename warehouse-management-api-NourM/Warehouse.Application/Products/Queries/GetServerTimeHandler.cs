using MediatR;

namespace Warehouse.Application.Products.Queries;

public class GetServerTimeHandler : IRequestHandler<GetServerTimeQuery, string>
{
    public Task<string> Handle(GetServerTimeQuery request, CancellationToken cancellationToken)
    {
        string time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

        return Task.FromResult($"Server time: {time}");
    }
}