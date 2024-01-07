using Microsoft.Extensions.DependencyInjection;
using Bookify.Domain.Bookings;
using Bookify.Application.Abstractions.Behaviors;
using FluentValidation;

namespace Bookify.Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplicatin(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependecyInjection).Assembly);
                configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
                configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(DependecyInjection).Assembly);

            services.AddTransient<PricingService>();

            return services;
        }
    }
}
