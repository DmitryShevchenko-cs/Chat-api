using Chat.DAL;
using Chat.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Chat.Test.Services
{
    public abstract class DefaultServiceTest<TService> where TService : class
    {
        protected IServiceProvider ServiceProvider;
        protected IServiceCollection ServiceCollection;
        
        protected virtual void SetUpAdditionalDependencies(IServiceCollection services)
        {
            services.AddScoped<TService>();
            services.AddAutoMapper(typeof(Startup));
        }

        [SetUp]
        public virtual void SetUp()
        {
            ServiceCollection = new ServiceCollection();
            ServiceCollection.AddDbContext<ChatDbContext>(options =>
                options.UseInMemoryDatabase("TestChatDB"));
            
            SetUpAdditionalDependencies(ServiceCollection);
            ServiceCollection.AddScoped<ChatDbContext>();

            var rootServiceProvider = ServiceCollection.BuildServiceProvider(new ServiceProviderOptions()
                { ValidateOnBuild = true, ValidateScopes = true });

            var spScope = rootServiceProvider.CreateScope();
            ServiceProvider = spScope.ServiceProvider;
        }
    }

    public abstract class DefaultServiceTest<TServiceInterface, TService> : DefaultServiceTest<TService>
        where TService : class, TServiceInterface where TServiceInterface : class
    {
        protected TService Service => ServiceProvider.GetRequiredService<TService>();
        protected override void SetUpAdditionalDependencies(IServiceCollection services)
        {
            services.AddScoped<TServiceInterface, TService>();
            base.SetUpAdditionalDependencies(services);
        }
    }
}