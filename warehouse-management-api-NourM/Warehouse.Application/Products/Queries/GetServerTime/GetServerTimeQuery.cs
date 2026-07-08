using MediatR;

namespace Warehouse.Application.Products.Queries;

public class GetServerTimeQuery : IRequest<string>
{
    public string Language { get; set; }

    public GetServerTimeQuery(string language)
    {
        Language = language;
    }
}