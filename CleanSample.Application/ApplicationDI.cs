using CleanSample.Application.Abstractions.Messaging;
using CleanSample.Domain.Abstractions.Event;
using CleanSample.Domain.Events;

namespace CleanSample.Application;

public static class ApplicationDi
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ICommandHandler<>).Assembly));
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}