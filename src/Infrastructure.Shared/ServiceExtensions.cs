using Core.Domain.Shared.Contacts;
using Infrastructure.Shared.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Shared
{
    public static class ServiceExtensions
    {
        public static void AddSharedInfrastructure(this IServiceCollection services)
        {
            #region Repositories
            services.AddTransient(typeof(IFileManagementRepository), typeof(FileManagementRepository));

            #endregion Repositories
        }

    }
}
