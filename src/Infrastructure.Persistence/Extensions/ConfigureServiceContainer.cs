using Core.Domain.Persistence.Contracts;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Extensions
{
    public static class ConfigureServiceContainer
    {
        public static void AddPersistenceDbContext(this IServiceCollection services, IConfiguration configuration,string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
        }

        public static void AddPersistenceRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient<IPersistenceUnitOfWork, PersistenceUnitOfWork>();
        }
    }
}
