using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Unistream.TransactionsApi.WebExtensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Конфигурация swagger
        /// </summary>
        /// <param name="serviceCollection">Доступные сервисы</param>
        /// <param name="applicationName">Имя приложения</param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerEx(this IServiceCollection serviceCollection, string applicationName)
        {
            serviceCollection.AddSwagger(applicationName,
                options =>
                {
                    string basePath = AppContext.BaseDirectory;
                    options.IncludeXmlComments(Path.Combine(basePath, "Unistream.TransactionsApi.xml"));
                    options.SchemaGeneratorOptions = new SchemaGeneratorOptions { SchemaIdSelector = type => type.FullName };
                }
            );

            return serviceCollection;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, string applicationName, Action<SwaggerGenOptions> configure = null)
        {
            services.AddApiVersioning(
                options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                }
            );

            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                }
            );

            services.AddSwaggerGen(
                options =>
                {
                    IApiVersionDescriptionProvider provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(description.GroupName, new OpenApiInfo
                        {
                            Title = applicationName,
                            Version = description.ApiVersion.ToString()
                        });
                    }
                    options.MapType<Guid>(() => new OpenApiSchema { Type = "string", Format = "uuid" });
                    options.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "" });
                    options.MapType<decimal?>(() => new OpenApiSchema { Type = "number", Format = "" });

                    configure?.Invoke(options);
                }
            );

            return services;
        }

        ///// <summary>
        ///// Метод по добавлению контекстов
        ///// </summary>
        ///// <param name="services"></param>
        //public static IServiceCollection AddInMemoryDatabaseConnections(this IServiceCollection services, IConfiguration configuration)
        //{
        //    // Добавление контекста ЛИС
        //    services.AddScoped<ILisContext>(serviceProvider =>
        //    {
        //        var lisContextFactory = serviceProvider.GetService<ILisContextFactory>();
        //        return lisContextFactory.GetTypedContext<LisContext>(options =>
        //            options.UseLazyLoadingProxies().UseInMemoryDatabase("Lis"));
        //    });

        //    return services;
        //}

        /// <summary>
        /// Метод по добавлению контекстов
        /// </summary>
        /// <param name="services"></param>
        //public static IServiceCollection AddRealDatabaseConnections(this IServiceCollection services, IConfiguration configuration)
        //{
        //    // Добавление контекста ЛИС
        //    services.AddScoped<ILisContext>(serviceProvider =>
        //    {
        //        var lisContextFactory = serviceProvider.GetService<ILisContextFactory>();
        //        return lisContextFactory.GetTypedContext<LisContext>(options =>
        //            options.UseLazyLoadingProxies()
        //                .UseNpgsql(configuration.GetConnectionString("LisDBConnectionString"),
        //                    x => x.CommandTimeout(3600).EnableRetryOnFailure()));
        //    });

        //    services.AddScoped<ResultsContext>(serviceProvider =>
        //    {
        //        var lisContextFactory = serviceProvider.GetService<Invitro.Ethereum.Audit.Abstractions.Context.IDbContextFactory<ResultsContext>>();
        //        return lisContextFactory.GetTypedContext<ResultsContext>(options =>
        //            options.UseLazyLoadingProxies()
        //                .LogTo(Log.Logger.Information, LogLevel.Information, null)
        //                .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        //                .EnableSensitiveDataLogging()
        //                .UseNpgsql(configuration.GetConnectionString("TaskControl")));
        //    });

        //    services.AddScoped<RegistrationContext>(serviceProvider =>
        //    {
        //        var lisContextFactory = serviceProvider.GetService<Invitro.Ethereum.Audit.Abstractions.Context.IDbContextFactory<RegistrationContext>>();
        //        return lisContextFactory.GetTypedContext<RegistrationContext>(options =>
        //            options.UseLazyLoadingProxies()
        //                .UseNpgsql(configuration.GetConnectionString("Registration")));
        //    });

        //    //TODO: осталось ли использование аудита?
        //    services.AddDbContext<AuditLisContext, ResultsAuditContext>(option =>
        //        option.UseNpgsql(configuration.GetConnectionString("TaskControl"),
        //            sqlOpt => sqlOpt.CommandTimeout(3600)));

        //    services.AddDbContext<SecurityContext>(
        //        builder =>
        //        {
        //            builder.UseLazyLoadingProxies()
        //                .UseNpgsql(configuration.GetConnectionString("Security"));
        //        }
        //    );

        //    services.AddScoped<ResultsHistoryContext>(serviceProvider =>
        //    {
        //        var lisContextFactory = serviceProvider.GetService<Invitro.Ethereum.Audit.Abstractions.Context.IDbContextFactory<ResultsHistoryContext>>();
        //        return lisContextFactory.GetTypedContext<ResultsHistoryContext>(options =>
        //            options.UseLazyLoadingProxies()
        //                .UseNpgsql(configuration.GetConnectionString("ResultsHistory"),
        //                    x => x.CommandTimeout(3600).EnableRetryOnFailure()));
        //    });

        //    services.AddDbContext<ResultsHistoryAuditContext>(option =>
        //        option.UseNpgsql(configuration.GetConnectionString("ResultsHistory"),
        //            sqlOpt => sqlOpt.CommandTimeout(3600)), ServiceLifetime.Transient);

        //    return services;
        //}

        /// <summary>
        /// Метод по добавлению фабрик
        /// </summary>
        /// <param name="services"></param>
        //public static IServiceCollection AddDatabaseConnectionsFactory(this IServiceCollection services, IConfiguration configuration)
        //{
        //    // Добавление фабрики ЛИС
        //    services.AddSingleton<ILisContextFactory>(sp =>
        //    {
        //        DbContextOptionsBuilder<LisContext> optionsBuilder = new DbContextOptionsBuilder<LisContext>();

        //        LisContextFactory factory = (LisContextFactory)new LisContextFactory(optionsBuilder.Options);

        //        return factory;
        //    });

        //    services.AddSingleton<Invitro.Ethereum.Audit.Abstractions.Context.IDbContextFactory<ResultsContext>>(sp =>
        //    {
        //        var optionsBuilder = new DbContextOptionsBuilder<ResultsContext>()
        //            .UseLazyLoadingProxies()
        //            .UseNpgsql(configuration.GetConnectionString("TaskControl"));

        //        ResultsContextFactory factory = new ResultsContextFactory(optionsBuilder.Options);

        //        return factory;
        //    });

        //    services.AddSingleton<Invitro.Ethereum.Audit.Abstractions.Context.IDbContextFactory<RegistrationContext>>(sp =>
        //    {
        //        IHttpContextAccessor accessor = sp.GetService<IHttpContextAccessor>();

        //        var optionsBuilder = new DbContextOptionsBuilder<RegistrationContext>()
        //                .UseNpgsql(configuration.GetConnectionString("Registration"));

        //        RegistrationContextFactory factory = (RegistrationContextFactory)new RegistrationContextFactory(optionsBuilder.Options,
        //                isAutoDetectChangesEnabled: true);

        //        return factory;
        //    });

        //    services.AddSingleton<Invitro.Ethereum.Audit.Abstractions.Context.IDbContextFactory<ResultsHistoryContext>>(sp =>
        //    {
        //        var optionsBuilder = new DbContextOptionsBuilder<ResultsHistoryContext>()
        //                .UseLazyLoadingProxies()
        //                .UseNpgsql(configuration.GetConnectionString("ResultsHistory"),
        //                    x => x.CommandTimeout(3600).EnableRetryOnFailure());

        //        ResultsHistoryContextFactory factory = new ResultsHistoryContextFactory(optionsBuilder.Options,
        //                isAutoDetectChangesEnabled: true);

        //        return factory;
        //    });

        //    services.AddSingleton<Audit.Abstractions.Context.IDbContextFactory<ResultsHistoryContext>>(sp =>
        //    {
        //        IHttpContextAccessor accessor = sp.GetService<IHttpContextAccessor>();

        //        var optionsBuilder = new DbContextOptionsBuilder<ResultsHistoryContext>()
        //                .UseLazyLoadingProxies()
        //                .UseNpgsql(configuration.GetConnectionString("ResultsHistory"),
        //                    x => x.CommandTimeout(3600).EnableRetryOnFailure());

        //        ResultsHistoryContextFactory factory = (ResultsHistoryContextFactory)new ResultsHistoryContextFactory(
        //                optionsBuilder.Options, isAutoDetectChangesEnabled: true)
        //            .WithAuditQueuePublish(() => accessor.HttpContext?.User?.UserDetails(),
        //                sp.GetService<IServiceScopeFactory>(),
        //                "ResultsControl",
        //                sp.GetService<ILogger<ResultsHistoryAuditContext>>(),
        //                saveToDatabase: false);

        //        return factory;
        //    });


        //    return services;
        //}
    }
}
