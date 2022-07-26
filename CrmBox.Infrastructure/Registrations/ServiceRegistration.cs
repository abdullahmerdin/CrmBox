using CrmBox.Application.Interfaces;
using CrmBox.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CrmBox.Infrastructure.Registrations;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {

        services.AddScoped<ICustomerService, CustomerService>();

    }
    
}