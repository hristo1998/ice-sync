using ContosoUniversity.DAL;
using IceSync.Core.Services;
using IceSync.Core.Services.Interfaces;
using IceSync.Core.SyncSerice;
using IceSync.Core.SyncService;
using IceSync.Core.SyncService.Interfaces;
using IceSync.Data;
using IceSync.Data.Entities;
using IceSync.Data.Repositories;
using IceSync.Data.Repositories.Interfaces;

namespace IceSync.Api.Extensions
{
    public static class ServiceBuilderExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Http Client
            services.AddHttpClient();
            
            // Services
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUniversalLoaderService, UniversalLoaderService>();
            
            // Scheduled Service
            services.AddHostedService<WorkflowSyncService>();

            // Caching
            services.AddMemoryCache();
            services.AddSingleton<IHashStore, MemoryHashStore>();

            // UnitOfWork and Repositories
            services.AddScoped<IIceSyncUnitOfWork, IceSyncUnitOfWork>();
            services.AddScoped<IRepository<Workflow>, Repository<Workflow>>();

            return services;
        }
    }
}
