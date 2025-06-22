using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_Go_Repository;
using FloraFauna_GO_Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

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
                        options.UseMySql($"{connectionString}", new MySqlServerVersion(new Version(10, 11, 1)))
                    , ServiceLifetime.Singleton);
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
                    .AddSingleton<
                        IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities, UtilisateurEntities,
                            SuccesEntities, SuccesStateEntities, LocalisationEntities>>(provider =>
                        new UnitOfWork(provider.GetRequiredService<FloraFaunaGoDB>()));
                break;

            default:
                services
                    .AddSingleton<
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
        services.AddAuthorization();

        var key = config["Jwt:Key"] ?? "FloraFaunaVeryHiddenKeyForJWTGeneration2025!";
        var issuer = config["Jwt:Issuer"] ?? "FloraFaunaIssuer";

        services.AddIdentity<UtilisateurEntities, IdentityRole>()
            .AddEntityFrameworkStores<FloraFaunaGoDB>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // Configuration des validations
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = issuer,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ClockSkew = TimeSpan.FromMinutes(1)
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var authHeader = context.Request.Headers["Authorization"].ToString();
                    Console.WriteLine($"OnMessageReceived - Auth Header brut: '{authHeader}'");

                    if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                    {
                        var rawToken = authHeader.Substring(7).Trim();

                        // Vérification explicite du format du token
                        var dotCount = rawToken.Count(c => c == '.');
                        Console.WriteLine($"Nombre de points dans le token: {dotCount}");

                        if (dotCount == 2)
                        {
                            context.Token = rawToken;
                            Console.WriteLine($"Token format valide: {rawToken.Substring(0, Math.Min(20, rawToken.Length))}...");
                        }
                        else
                        {
                            Console.WriteLine($"ATTENTION: Format de token incorrect ({dotCount} points au lieu de 2)");
                            context.Token = null; // Force l'échec de l'authentification
                        }
                    }

                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine($"Token validé avec succès! Utilisateur: {context.Principal?.Identity?.Name ?? "inconnu"}");
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Échec d'authentification: {context.Exception}");
                    return Task.CompletedTask;
                }
            };
        });
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

            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Description = "Entrer 'Bearer {token}'",
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    jwtSecurityScheme,
                    Array.Empty<string>()
                }
            });
        });
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();

        // Middleware de diagnostic global - pour toutes les requêtes
        app.Use(async (context, next) =>
        {
            Console.WriteLine($"\n===== REQUÊTE ENTRANTE: {context.Request.Method} {context.Request.Path} =====");

            // Capture les headers pour le diagnostic
            foreach (var header in context.Request.Headers)
            {
                Console.WriteLine($"Header: {header.Key}: {header.Value}");
            }

            // Vérification JWT si présent
            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                if (authHeader.ToString().StartsWith("Bearer "))
                {
                    var token = authHeader.ToString().Substring(7).Trim();
                    Console.WriteLine($"Token JWT présent: {token.Substring(0, Math.Min(20, token.Length))}...");
                    Console.WriteLine($"Points dans le token: {token.Count(c => c == '.')}");

                    // Afficher une représentation hexadécimale pour détecter des caractères invisibles
                    Console.WriteLine($"Premier 30 caractères en hex: {BitConverter.ToString(Encoding.UTF8.GetBytes(token.Substring(0, Math.Min(30, token.Length))))}");
                }
            }

            // Création d'un middleware de réponse pour capturer le code de statut
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await next();

            // Afficher le résultat après exécution du pipeline
            Console.WriteLine($"===== RÉPONSE: {context.Response.StatusCode} =====");

            // Restaurer le corps de la réponse
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapHealthChecks("/health");

        // Configure the HTTP request pipeline.
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