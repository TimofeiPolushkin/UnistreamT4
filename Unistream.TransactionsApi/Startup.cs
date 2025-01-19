using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Unistream.TransactionsApi.WebExtensions;
using Unistream.TransactionsApi.ErrorHandling;
using Unistream.Transactions.Model.Interfaces;
using Unistream.Transactions.Model.EF;
using Unistream.TransactionsApi.Services;
using Serilog;
using Unistream.TransactionsApi.Middleware;

namespace Unistream.TransactionsApi
{
    public class Startup
    {
        private const string ApplicationName = "unistream-transactions-api";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddLogging();
            services.AddOptions();

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.ReportApiVersions = true;
            });

            services
                .AddSwaggerEx(ApplicationName)
                .AddMvcCore().ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = c =>
                    {
                        string errors = string.Join('\n', c.ModelState.Values.Where(v => v.Errors.Count > 0)
                            .SelectMany(v => v.Errors)
                            .Select(v => v.ErrorMessage));

                        return new BadRequestObjectResult(new TransactionsApiError
                        {
                            Message = errors,
                            ErrorCode = ErrorCode.WrongRequest
                        });
                    };
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddRealDatabaseConnectionsFactory(Configuration);
            services.AddRealDatabaseConnections(Configuration);

            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionProcessingService, TransactionProcessingService>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider versionProvider, System.IServiceProvider sp)
        {
            loggerFactory.AddSerilog();

            app.UsePathBase("/transactions")
               .UseRouting()
               .UseCors(builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Content-Disposition"));

            app.UseMiddleware<ErrorHandlingMiddleware>()
                .UseRequestLocalization()
                .UseEndpoints(builder =>
                {
                    builder.MapControllers();
                })
                .UseStaticFiles()
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    foreach (var description in versionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant()
                        );
                    }

                    options.DisplayOperationId();
                });
        }
    }
}
