using AdventureArchive.Api.Domain.Interfaces;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;
using AdventureArchive.Api.Infrastructure.Repositories;

namespace AdventureArchive.Api.Api.Extensions;

public static class ServiceExtensions
{
    public static void RegisterDependencies(this IServiceCollection services, bool isDevelopment = false)
    {
        services.AddHttpClient<IDocHttpClient, DocHttpClient>("DocHttpClient")
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var httpProxy = Environment.GetEnvironmentVariable("HTTP_PROXY");
                if (isDevelopment && !string.IsNullOrEmpty(httpProxy))
                {
                    return new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                }

                return new HttpClientHandler();
            });
        services.AddScoped<IDocService, DocService>();
        services.AddScoped<IHutProvider, HutRepository>();
        services.AddScoped<ITrackProvider, TrackRepository>();
        
    }
}