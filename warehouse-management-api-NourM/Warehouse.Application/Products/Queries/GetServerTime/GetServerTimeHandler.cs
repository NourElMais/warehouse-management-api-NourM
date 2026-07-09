using System.Globalization;
using MediatR;

namespace Warehouse.Application.Products.Queries;

public class GetServerTimeHandler : IRequestHandler<GetServerTimeQuery, string>
{
    public Task<string> Handle(
        GetServerTimeQuery request,
        CancellationToken cancellationToken)
    {
        DateTime now = DateTime.UtcNow;

        string result;

        if (request.Language == "en-US")
        {
            result = now.ToString(
                "MMMM dd, yyyy hh:mm:ss tt",
                new CultureInfo("en-US"));
        }
        else if (request.Language == "fr-FR")
        {
            result = now.ToString(
                "dd MMMM yyyy HH:mm:ss",
                new CultureInfo("fr-FR"));
        }
        else if (request.Language == "ar-LB")
        {
            result = now.ToString(
                "dd MMMM yyyy HH:mm:ss",
                new CultureInfo("ar-LB"));
        }
        else
        {
            result = now.ToString();
        }

        return Task.FromResult(result);
    }
}