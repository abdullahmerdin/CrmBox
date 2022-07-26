using Autofac;
using Autofac.Extensions.DependencyInjection;
using CrmBox.Core.Domain.Identity;
using CrmBox.Infrastructure.Extensions.ExceptionHandler;
using CrmBox.Infrastructure.Registrations;
using CrmBox.Persistance.Context;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Serilog;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new CrmBox.Infrastructure.Registrations.AutofacRegistration());
    });

builder.Services.AddClaimAuthorizationPolicies();

//Add Serilog
Log.Logger = new LoggerConfiguration().CreateLogger();
builder.Host.UseSerilog(((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.Console()));

//Add DbContext
builder.Services.AddDbContext<CrmBoxIdentityContext>();
builder.Services.AddDbContext<CrmBoxContext>();

builder.Services.AddDbContext<CrmBoxLogContext>();

//Add Localization
builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });

builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("tr"),
        new CultureInfo("en")
    };
    opt.DefaultRequestCulture = new RequestCulture("tr");
    opt.SupportedCultures = supportedCultures;
    opt.SupportedUICultures = supportedCultures;
});


//Add Identity
builder.Services.AddIdentity<AppUser, AppRole>(x =>
    {
        x.Password.RequireUppercase = false;
        x.Password.RequireNonAlphanumeric = false;
        x.Password.RequireDigit = false;
        x.Password.RequireLowercase = false;
        x.Password.RequiredLength = 4;
    })
    .AddEntityFrameworkStores<CrmBoxIdentityContext>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandlerMiddleware();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseRequestLocalization(((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customers}/{action=Add}/{id?}");

app.Run();
