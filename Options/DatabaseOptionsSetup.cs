using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace WEB_API.Options;

public sealed class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private const string ConnectionString = "Database";
    private readonly IConfiguration _configuration;

    public DatabaseOptionsSetup(IConfiguration configuration) => _configuration = configuration;

    public void Configure(DatabaseOptions options)
    {
        options.ConnectionString = _configuration.GetConnectionString(name: ConnectionString)!;

        _configuration
            .GetRequiredSection(key: "DatabaseOptions")
            .Bind(instance: options);
    }
}
