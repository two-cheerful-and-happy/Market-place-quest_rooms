using Microsoft.AspNetCore.Components.Authorization;

namespace UI;

public static class Initializer
{
    public static void InitializeRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<Location>, LocationRepository>();
        services.AddScoped<IBaseRepository<LocationOfUser>, LocationOfUserRepository>();
        services.AddScoped<IBaseRepository<Account>, AccountRepository>();
    }

    public static void InitializeServices(this IServiceCollection services)
    {
        services.AddSingleton<IMailService, MailService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IMapService, MapService>();
    }
}