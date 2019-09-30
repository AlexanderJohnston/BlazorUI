using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BlazorUI.Service;
using BlazorUI.Service.Models;

namespace BlazorUI.Service
{
    public class ServiceSecrets
    {
        public ApplicationOptions Options { get; set; }

        public ServiceSecrets(IConfiguration config)
        {
            Configuration = config;
            Options = new ApplicationOptions();
        }
        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            Options.DealerOnConnectionString = Configuration.GetSection("LegacyEvents").Get<LegacyDatabase>().ConnectionString;
            services.AddSingleton(s => new TorqueQAContext(Options));
        }
    }

    public class LegacyDatabase
    {
        public string ConnectionString { get; set; }
    }
}