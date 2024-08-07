using Chat.BLL.Services;
using Chat.BLL.Services.Interfaces;
using Chat.DAL;
using Chat.DAL.Repositories;
using Chat.DAL.Repositories.Interfaces;
using Chat.Web.Hubs;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

namespace Chat.Web;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(opt => 
            opt.SerializerSettings.Converters.Add(new StringEnumConverter()));
        
        services.AddSignalR().AddHubOptions<RoomHub>(options =>
        {
            options.EnableDetailedErrors = true;
        });
        
        //automapper
        services.AddAutoMapper(typeof(Startup));
        
        // DI Repositories
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        // DI Services
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IUserService, UserService>();
        
        var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING") ?? Configuration.GetConnectionString("ConnectionString");

        services.AddDbContext<ChatDbContext>(options =>
            options.UseSqlServer(connectionString));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        app.UseRouting();
        
        app.UseStaticFiles();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapHub<RoomHub>("/roomHub", options =>
            {
                options.Transports = HttpTransportType.LongPolling | HttpTransportType.WebSockets;
            });
            
        });
        
    }
}