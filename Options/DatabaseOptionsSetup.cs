using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace WEB_API.Options;

public sealed class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private readonly IConfiguration _configuration;

    public DatabaseOptionsSetup(IConfiguration configuration) => _configuration = configuration;

    public void Configure(DatabaseOptions options)
    {
        _configuration
            .GetRequiredSection(key: "DatabaseOptions")
            .Bind(instance: options);
    }
}
