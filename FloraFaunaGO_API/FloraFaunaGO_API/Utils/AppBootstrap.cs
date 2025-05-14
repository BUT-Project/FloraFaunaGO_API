using System.Reflection;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_Go_Repository;
using FloraFauna_GO_Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FloraFaunaGO_API.Utils;

public class AppBootstrap(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        AddSwagger(services);
        AddFloraFaunaGoContextServices(services);
        AddModelService(services);
        AddIdentityServices(services, configuration);
        services.AddHealthChecks();
    }

    private void AddFloraFaunaGoContextServices(IServiceCollection services)
    {
        string? connectionString;

        switch (Environment.GetEnvironmentVariable("TYPE"))
        {
            case "BDD":
                var host = Environment.GetEnvironmentVariable("HOST");
                var port = Environment.GetEnvironmentVariable("PORTDB");
                var database = Environment.GetEnvironmentVariable("DATABASE");
                var username = Environment.GetEnvironmentVariable("USERNAME");
                var password = Environment.GetEnvironmentVariable("PASSWORD");

                connectionString = $"Server={host};port={port};database={database};user={username};password={password}";
                Console.WriteLine("========RUNNING USING THE MYSQL SERVER==============");
                Console.WriteLine(connectionString);
                Console.WriteLine("====================================================");
                services.AddDbContext<FloraFaunaGoDB>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)),
                        ServiceLifetime.Singleton);
                break;
            default:
                Console.WriteLine("====== RUNNING USING THE IN SQLITE DATABASE ======");
                connectionString = Configuration.GetConnectionString("FloraFaunaGoConnection");
                Console.WriteLine(connectionString);
                if (!string.IsNullOrWhiteSpace(connectionString))
                    services.AddDbContext<FloraFaunaGoDB>(options =>
                        options.UseSqlite(connectionString), ServiceLifetime.Singleton);
                else
                    services.AddDbContext<FloraFaunaGoDB>();

                break;
        }
    }

    private void AddModelService(IServiceCollection services)
    {
        switch (Environment.GetEnvironmentVariable("TYPE"))
        {
            case "BDD":
                services
                    .AddScoped<
                        IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities, UtilisateurEntities,
                            SuccesEntities, SuccesStateEntities, LocalisationEntities>>(provider =>
                        new UnitOfWork(provider.GetRequiredService<FloraFaunaGoDB>()));
                break;

            default:
                services
                    .AddScoped<
                        IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities, UtilisateurEntities,
                            SuccesEntities, SuccesStateEntities, LocalisationEntities>>(provider =>
                    {
                        provider.GetRequiredService<FloraFaunaGoDB>().Database.EnsureCreated();
                        return new UnitOfWork(provider.GetRequiredService<FloraFaunaGoDB>());
                    });
                break;
        }

        services.AddScoped<FloraFaunaService>(provider =>
            new FloraFaunaService(provider
                .GetRequiredService<
                    IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities, UtilisateurEntities,
                        SuccesEntities, SuccesStateEntities, LocalisationEntities>>()));
    }

    private void AddIdentityServices(IServiceCollection services, IConfiguration config)
    {
        // [DAve] pk ya pas le bg svp ??
    }

    private void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //{
            //    Description =
            //        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            //    Name = "Authorization",
            //    In = ParameterLocation.Header,
            //    Type = SecuritySchemeType.Http,
            //    Scheme = "Bearer",
            //    BearerFormat = "JWT"
            //});
            //var scheme = new OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "Bearer"
            //            },
            //            Scheme = "oauth2",
            //            Name = "Bearer",
            //            In = ParameterLocation.Header
            //        },
            //        new List<string>()
            //    }
            //};

            //options.AddSecurityRequirement(scheme);
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "FloraFaunaGO API",
                Description = "API documentation for FloraFaunaGO",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "FFGO Contact",
                    Url = new Uri("https://example.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "Mit License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });
        });
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

//        app.MapIdentityApi<AthleteEntity>();

        app.MapControllers();

        app.MapHealthChecks("/health");

        // Configure the HTTP request pipeline.
        if (true)
        {
            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    if (httpReq.Headers.ContainsKey("X-Forwarded-Host"))
                    {
                        string basePath;
                        switch (Environment.GetEnvironmentVariable("TYPE")) // httpReq.Host.Value
                        {
                            case "BDD":
                                basePath = "containers/FloraFauna_GO-api";
                                break;
                            default:
                                basePath = httpReq.Host.Value;
                                break;
                        }

                        var serverUrl = $"https://{httpReq.Headers["X-Forwarded-Host"]}/{basePath}";
                        swagger.Servers = new List<OpenApiServer> { new() { Url = serverUrl } };
                    }
                });
            });
            app.UseSwaggerUI();
            app.MapSwagger();
        }
    }
}