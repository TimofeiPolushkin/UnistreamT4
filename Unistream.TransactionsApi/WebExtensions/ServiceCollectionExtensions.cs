using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Unistream.Transactions.Model.EF;
using Microsoft.EntityFrameworkCore;
using Unistream.Clients.Model.EF;

namespace Unistream.TransactionsApi.WebExtensions
{
    /// <summary>
    /// Методы расширения зависимостей
    /// </summary>
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

        /// <summary>
        /// Конфигурация swagger
        /// </summary>
        /// <param name="applicationName">Имя приложения</param>
        /// <returns></returns>
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

        /// <summary>
        /// Метод по добавлению контекстов
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddInMemoryDatabaseConnections(this IServiceCollection services, IConfiguration configuration)
        {
            // Добавление контекста транзакций
            services.AddScoped<TransactionContext>(serviceProvider =>
            {
                var transactionFactory = serviceProvider.GetService<TransactionContextFactory>();
                return transactionFactory.CreateDbContext<TransactionContext>(options =>
                    options.UseLazyLoadingProxies().UseInMemoryDatabase("TransactionsHistory"));
            });

            // Добавление контекста клиентов
            services.AddScoped<ClientContext>(serviceProvider =>
            {
                var transactionFactory = serviceProvider.GetService<ClientContextFactory>();
                return transactionFactory.CreateDbContext<ClientContext>(options =>
                    options.UseLazyLoadingProxies().UseInMemoryDatabase("Clients"));
            });


            return services;
        }

        /// <summary>
        /// Метод по добавлению фабрик
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddInMemoryDatabaseConnectionsFactory(this IServiceCollection services, IConfiguration configuration)
        {
            // Добавление фабрики транзакций
            services.AddSingleton<TransactionContextFactory>(sp =>
            {
                DbContextOptionsBuilder<TransactionContext> optionsBuilder = new DbContextOptionsBuilder<TransactionContext>();

                TransactionContextFactory factory = (TransactionContextFactory)new TransactionContextFactory(optionsBuilder.Options);

                return factory;
            });

            services.AddSingleton<IDbContextFactory<TransactionContext>>(sp =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<TransactionContext>()
                    .UseLazyLoadingProxies()
                    .UseInMemoryDatabase("TransactionsHistory");

                TransactionContextFactory factory = new TransactionContextFactory(optionsBuilder.Options);

                return factory;
            });

            // Добавление фабрики клиентов
            services.AddSingleton<ClientContextFactory>(sp =>
            {
                DbContextOptionsBuilder<ClientContext> optionsBuilder = new DbContextOptionsBuilder<ClientContext>();

                ClientContextFactory factory = (ClientContextFactory)new ClientContextFactory(optionsBuilder.Options);

                return factory;
            });

            services.AddSingleton<IDbContextFactory<ClientContext>>(sp =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ClientContext>()
                    .UseLazyLoadingProxies()
                    .UseInMemoryDatabase("Clients");

                ClientContextFactory factory = new ClientContextFactory(optionsBuilder.Options);

                return factory;
            });

            return services;
        }

        /// <summary>
        /// Метод по добавлению контекстов
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddRealDatabaseConnections(this IServiceCollection services, IConfiguration configuration)
        {
            // Добавление контекста транзакций
            services.AddScoped<TransactionContext>(serviceProvider =>
            {
                var lisContextFactory = serviceProvider.GetService<TransactionContextFactory>();
                return lisContextFactory.CreateDbContext<TransactionContext>(options =>
                    options.UseLazyLoadingProxies()
                        .UseNpgsql(configuration.GetConnectionString("TransactionsHistory"),
                            x => x.CommandTimeout(3600).EnableRetryOnFailure()));
            });

            // Добавление контекста клиентов
            services.AddScoped<ClientContext>(serviceProvider =>
            {
                var lisContextFactory = serviceProvider.GetService<ClientContextFactory>();
                return lisContextFactory.CreateDbContext<ClientContext>(options =>
                    options.UseLazyLoadingProxies()
                        .UseNpgsql(configuration.GetConnectionString("Clients"),
                            x => x.CommandTimeout(3600).EnableRetryOnFailure()));
            });

            return services;
        }

        /// <summary>
        /// Метод по добавлению фабрик
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddRealDatabaseConnectionsFactory(this IServiceCollection services, IConfiguration configuration)
        {
            // Добавление фабрики транзакций
            services.AddSingleton<TransactionContextFactory>(sp =>
            {
                DbContextOptionsBuilder<TransactionContext> optionsBuilder = new DbContextOptionsBuilder<TransactionContext>();

                TransactionContextFactory factory = (TransactionContextFactory)new TransactionContextFactory(optionsBuilder.Options);

                return factory;
            });

            services.AddSingleton<IDbContextFactory<TransactionContext>>(sp =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<TransactionContext>()
                    .UseLazyLoadingProxies()
                    .UseNpgsql(configuration.GetConnectionString("TransactionsHistory"));

                TransactionContextFactory factory = new TransactionContextFactory(optionsBuilder.Options);

                return factory;
            });

            // Добавление фабрики клиентов
            services.AddSingleton<ClientContextFactory>(sp =>
            {
                DbContextOptionsBuilder<ClientContext> optionsBuilder = new DbContextOptionsBuilder<ClientContext>();

                ClientContextFactory factory = (ClientContextFactory)new ClientContextFactory(optionsBuilder.Options);

                return factory;
            });

            services.AddSingleton<IDbContextFactory<ClientContext>>(sp =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ClientContext>()
                    .UseLazyLoadingProxies()
                    .UseNpgsql(configuration.GetConnectionString("Clients"));

                ClientContextFactory factory = new ClientContextFactory(optionsBuilder.Options);

                return factory;
            });

            return services;
        }
    }
}
