using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PomodoroWebApp.Infrastructure.Data;
using PomodoroWebApp.Infrastructure.InitializationExtensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;


namespace PomodoroWebApp.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabaseConfiguration(configuration);
        services.AddDAOInjections();
        services.AddServicesInjections();
        services.AddInteractorsInjections();
        services.AddJwtAuthentication(configuration);
        services.AddSwagger();
        //services.AddRedisConfiguration(configuration);
        //services.AddLoggingConfiguration();
        services.AddControllers(options =>
        {
            options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
        });
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true; // Usar la versión por defecto si no se especifica
            options.DefaultApiVersion = ApiVersion.Default; // Versión por defecto
            options.ReportApiVersions = true; // Informar las versiones disponibles en la respuesta HTTP
        }).AddApiExplorer(option =>
        {
            option.GroupNameFormat = "'v'V";
            option.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    private static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var optionsConfig = configuration.GetOptions();

        services.AddSingleton(optionsConfig.SqlServerConfig);

        services.AddDbContext<PomodoroDbContext>(options =>
        {

            options.UseSqlServer(optionsConfig.SqlServerConfig.DefaultConnection,
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(

                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var optionsConfig = configuration.GetOptions();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = optionsConfig.JwtConfig.Issuer,
                ValidAudience = optionsConfig.JwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(optionsConfig.JwtConfig.Secret))
            };
        });

        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(configuracion =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                configuracion.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                configuracion.UseInlineDefinitionsForEnums();
            }

            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            configuracion.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            configuracion.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

            configuracion.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
            configuracion.DescribeAllParametersInCamelCase();

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                configuracion.IncludeXmlComments(xmlPath);
        });

        return services;
    }

    private class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                paths.Add(path.Key.Replace("{version}", swaggerDoc.Info.Version), path.Value);
            }
            swaggerDoc.Paths = paths;
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "PomodoroAppWeb",
            Version = description.ApiVersion.ToString(),
            Description = "API REST para la gestión de tareas y sesiones Pomodoro.",
            Contact = new OpenApiContact { Name = "Sergio Veizaga", Email = "sergio19101992@gmail.com" }
        };
        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }
        return info;
    }

    private static IServiceCollection AddDAOInjections(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection AddServicesInjections(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection AddInteractorsInjections(this IServiceCollection services)
    {
        return services;
    }

    /*
    private static IServiceCollection AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfig = configuration.GetOptions<RedisConfig>("RedisConfig");
        services.AddSingleton(redisConfig);
        return services;
    }

    private static IServiceCollection AddLoggingConfiguration(this IServiceCollection services)
    {
        services.AddLogging(builder => builder.AddConsole());
        return services;
    }
    */
}
