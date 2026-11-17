using Microsoft.EntityFrameworkCore;
using StorageApi.Data;
using StorageApi.Options;

namespace StorageApi.Extensions;

public static class RepositoryServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StorageApiContext>(options =>
        {
            var connectionString = configuration.GetSection(ConnectionStringOption.Key)
                .Get<ConnectionStringOption>();

            if (string.IsNullOrWhiteSpace(connectionString?.SqlServer))
            {
                throw new InvalidOperationException("Connection string 'ConnectionStrings:SqlServer' not found.");
            }

            options.UseSqlServer(connectionString.SqlServer, sqlServerOptionsAction =>
            {
                sqlServerOptionsAction.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.FullName);
            });
        });

        return services;
    }
}