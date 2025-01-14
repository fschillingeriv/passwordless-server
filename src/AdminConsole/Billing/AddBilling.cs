using AdminConsole.Services;
using Stripe;

namespace AdminConsole.Billing;

public static class AddBillingExtensionMethods
{
    public static IServiceCollection AddBilling(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddOptions<StripeOptions>()
            .BindConfiguration("Stripe");

        StripeConfiguration.ApiKey = builder.Configuration["Stripe:ApiKey"];

        services.AddScoped<SharedBillingService>();
        return services;
    }
}