using Autofac;
using CrmBox.Application.Interfaces;
using CrmBox.Application.Services;

namespace CrmBox.Infrastructure.Registrations;

public class AutofacRegistration : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CustomerService>().As<ICustomerService>();
        
    }
}