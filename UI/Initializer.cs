using Microsoft.AspNetCore.Components.Authorization;

namespace UI;

public static class Initializer
{
    public static void InitializeRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<Location>, LocationRepository>();
        services.AddScoped<IBaseRepository<Account>, AccountRepository>();
        services.AddScoped<IBaseRepository<Photo>, PhotoRepository>();
        services.AddScoped<IBaseRepository<Comment>, CommentRepository>();
    }

    public static void InitializeServices(this IServiceCollection services)
    {
        services.AddSingleton<IMailService, MailService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IMapService, MapService>();
        services.AddScoped<IAdminPanelService, AdminPanelService>();
    }
}