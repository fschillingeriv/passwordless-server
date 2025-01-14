using Microsoft.Extensions.Options;
using Passwordless.Net;

// This is a trick to always show up in a class when people are registering services
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPasswordlessSdk(this IServiceCollection services, Action<PasswordlessOptions> configureOptions)
    {
        services.AddOptions<PasswordlessOptions>()
            .Configure(configureOptions)
            .PostConfigure(options => options.ApiUrl ??= PasswordlessOptions.CloudApiUrl)
            .Validate(options => !string.IsNullOrEmpty(options.ApiKey) && !string.IsNullOrEmpty(options.ApiSecret), "Missing ApiKey and/or ApiSecret");

        // TODO: Use AddHttpMessageHandler for error handling
        services.AddHttpClient<IPasswordlessClient, PasswordlessClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<PasswordlessOptions>>().Value;

            client.BaseAddress = new Uri(options.ApiUrl);
            client.DefaultRequestHeaders.Add("ApiSecret", options.ApiSecret);
        });

        // TODO: Get rid of this service, all consumers should use the interface
        services.AddTransient(sp => (PasswordlessClient)sp.GetRequiredService<IPasswordlessClient>());

        return services;
    }
}