using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PomodoroWebApp.Application.Interactor;
using PomodoroWebApp.Application.Interfaces.Interactor;
using PomodoroWebApp.Application.Services;
using PomodoroWebApp.Domain.Entities;
using PomodoroWebApp.Domain.Interfaces.Repositories;
using PomodoroWebApp.Domain.Interfaces.Services;
using PomodoroWebApp.Infrastructure.Config.InitializationExtensions;
using PomodoroWebApp.Infrastructure.Config.Options;
using PomodoroWebApp.Infrastructure.Data;
using PomodoroWebApp.Infrastructure.Data.Repositories;
using PomodoroWebApp.Infrastructure.Services;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

namespace PomodoroWebApp.Infrastructure.Config.Extensions;

/// <summary>
/// Extension methods for configuring the service collection.
/// </summary>
public static class ServiceCollectionExtension
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLoggingConfiguration(configuration);
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddDataAccessInjections();
        services.AddServicesInjections();
        services.AddInteractorsInjections();
        services.AddIdentityConfiguration(configuration);
        services.AddSecurityPolicies();
        services.AddDatabaseConfiguration(configuration);
        services.AddJwtAuthentication(configuration);
        services.AddSwagger();
        services.AddCustomHealthChecks(configuration);
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
    private static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlServerConfig = configuration.GetOptions<SqlServerConfig>("SqlServerConfig");

        var healthChecksBuilder = services.AddHealthChecks();

        healthChecksBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

        if (!string.IsNullOrEmpty(sqlServerConfig.DefaultConnection))
        {
            healthChecksBuilder.AddSqlServer(sqlServerConfig.DefaultConnection, name: "sqlserver");
        }

        return services;
    }

    private static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var serilogConfig = configuration.GetOptions<SerilogConfig>("SerilogConfig");
        
        Log.Logger = new LoggerConfiguration()
         .MinimumLevel.Is(Enum.Parse<LogEventLevel>(serilogConfig.MinimumLevel))
         .WriteTo.Console(outputTemplate: serilogConfig.ConsoleOutputTemplate)
         .WriteTo.File(
             path: serilogConfig.LogFilePath,
             rollingInterval: RollingInterval.Day,
             retainedFileCountLimit: serilogConfig.RetainedFileCountLimit,
             outputTemplate: serilogConfig.FileOutputTemplate)
         .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });

        return services;
    }

    private static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var identityConfig = configuration.GetOptions<IdentityConfig>("IdentityConfig");

        services.AddIdentity<Usuario, IdentityRole<int>>(options =>
        {
            // Configuración de contraseña
            options.Password.RequireDigit = identityConfig.Password.RequireDigit;
            options.Password.RequiredLength = identityConfig.Password.RequiredLength;
            options.Password.RequireNonAlphanumeric = identityConfig.Password.RequireNonAlphanumeric;
            options.Password.RequireUppercase = identityConfig.Password.RequireUppercase;
            options.Password.RequireLowercase = identityConfig.Password.RequireLowercase;

            // Configuración de usuario
            options.User.RequireUniqueEmail = identityConfig.User.RequireUniqueEmail;

            // Configuración de bloqueo
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identityConfig.Lockout.DefaultLockoutTimeInMinutes);
            options.Lockout.MaxFailedAccessAttempts = identityConfig.Lockout.MaxFailedAccessAttempts;
        })
        .AddEntityFrameworkStores<PomodoroDbContext>()
        .AddDefaultTokenProviders();

        // Configuración de cookies seguras
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(identityConfig.Cookie.ExpireTimeInMinutes);
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.SlidingExpiration = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        return services;
    }

    private static IServiceCollection AddSecurityPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Política para usuarios autenticados
            options.AddPolicy("AuthenticatedUser", policy =>
                policy.RequireAuthenticatedUser());

            // Política para administradores
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));

            // Política para tokens JWT válidos
            options.AddPolicy("ValidToken", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
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
        var jwtConfig = configuration.GetOptions<JwtConfig>("JwtConfig");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
                ClockSkew = TimeSpan.Zero // Eliminar margen de tiempo para expiración
            };

            // Para manejar eventos del token
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                }
            };
        });

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
            {
                configuracion.IncludeXmlComments(xmlPath);
                configuracion.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PomodoroWebApp.xml"));
                configuracion.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PomodoroWebApp.Application.xml"));
            }
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

    private static IServiceCollection AddDataAccessInjections(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }

    private static IServiceCollection AddServicesInjections(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IIdentityService, IdentityService>();

        services.AddScoped<IAuthorizationTokenService, AuthorizationTokenService>();

        return services;
    }

    private static IServiceCollection AddInteractorsInjections(this IServiceCollection services)
    {
        #region POST

        services.AddScoped<IRegisterInteractor, RegisterInteractor>();
        services.AddScoped<ILoginInteractor, LoginInteractor>();

        #endregion

        #region GET
        #endregion

        #region PUT
        #endregion

        #region DELETE
        #endregion

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
