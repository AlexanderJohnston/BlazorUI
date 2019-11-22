using BlazorUI.Service.Data;
using BlazorUI.Service.Models;
using BlazorUI.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Totem.Runtime.Hosting;

namespace BlazorUI.Service
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationConfigured(this IServiceCollection services, string connection) =>
            services.AddApplication().AddDatabaseWithSecrets(connection);

        //public static IServiceCollection AddApplicationHardCoded(this IServiceCollection services) =>
        //    services.AddApplication().AddLegacyEventDatabase();

        static IServiceCollection AddApplication(this IServiceCollection services) =>
            services
            .AddHttpClient()
            .AddApplicationOptions();

        static IServiceCollection AddApplicationOptions(this IServiceCollection services) =>
            services.BindOptionsToConfiguration<ApplicationOptions>("app");

        /// <summary>
        ///     Legacy method of attaching the database. Hard-coded.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        //static IServiceCollection AddLegacyEventDatabase(this IServiceCollection services) =>
        //    services.AddSingleton<ILegacyEventContext, TorqueQAContext>(s => new TorqueQAContext(new DbContextOptionsBuilder<TorqueQAContext>().UseSqlServer("connectionString").Options));

        /// <summary>
        ///     User secrets method of attaching the database.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        static IServiceCollection AddDatabaseWithSecrets(this IServiceCollection services, string connection)
        {
            return services.AddSingleton<ILegacyEventContext, DatabaseService>(s => new DatabaseService(connection));
        }
    }
}
