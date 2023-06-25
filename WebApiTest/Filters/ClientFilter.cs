using Microsoft.FeatureManagement;

namespace WebApiTest.Filters;

public class ClientFilter : IFeatureFilter
{
    private readonly IConfiguration _configuration;

    public ClientFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
    {
        var clientName = _configuration["ClientName"];

        var validClients = context.Parameters.Get<List<string>>();

        return Task.FromResult(validClients.Contains(clientName));
    }
}