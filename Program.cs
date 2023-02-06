using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WEB_API;
using WEB_API.Options;

var builder = WebApplication.CreateBuilder(args: args);

/**
 *
 * Services
 *
 */
var services = builder.Services;

services
    .ConfigureOptions<DatabaseOptionsSetup>()
    .AddDbContext<BookStoreContext>(optionsAction: (serviceProvider, dbContextOptionsBuilder) =>
    {
        var databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

        dbContextOptionsBuilder
            .UseInMemoryDatabase(
                databaseName: "MyBookStore",
                inMemoryOptionsAction: actions => actions.EnableNullChecks())
            .EnableDetailedErrors(detailedErrorsEnabled: databaseOptions.EnableDetailedErrors)
            .EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
    })
    .AddSwaggerGen(setupAction: c => c.SwaggerDoc(
        name: "v1",
        info: new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BASIC_WEB_API", Version = "v1" }))
    .AddCors(setupAction: action => action.AddDefaultPolicy(configurePolicy: policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()))
    .AddControllers()
    .AddMvcOptions(setupAction: options => options.SuppressAsyncSuffixInActionNames = false);

var app = builder.Build();

/**
 *
 * Middlewares pipeline
 *
 */
if (app.Environment.IsDevelopment())
{
    app
        .UseSwagger()
        .UseSwaggerUI();
}
else
{
    app
        .UseExceptionHandler("/Error")
        .UseHsts();
}

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseCors();

app.MapControllers();

app.Run();