using Unistream.Transactions.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Unistream.TransactionsApi.WebExtensions;
using Unistream.TransactionsApi.ErrorHandling;

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
                            Message = errors
                        });
                    };
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddMediatR(Assembly.GetExecutingAssembly());

            //services.AddScoped<IRabbitService, RabbitService>();

            //services.AddScoped<IFieldService, FieldService>();

            //services.AddScoped<IMoveEventService, MoveEventService>();

            //services.AddScoped<IPastureEventService, PastureEventService>();

            //services.AddScoped<IPictureService, PictureService>();

            //services.AddScoped<ICalculateEnergyService, CalculateEnergyService>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApiVersionDescriptionProvider versionProvider, System.IServiceProvider sp)
        {
            //loggerFactory.AddSerilog();

            app.UsePathBase("/transactions")
               .UseRouting()
               .UseCors(builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Content-Disposition"));

            app.UseRequestLocalization()
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

            app.UseSpa(
                spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                }
            );
        }

        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();

        //        app.UseSwagger();

        //        app.UseSwaggerUI(c =>
        //        {
        //            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Energy Calculation");
        //        });
        //    }
        //    //app.UseMiddleware<ErrorHandlingMiddleware>();

        //    app.UseRouting();

        //    app.UseStaticFiles();

        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllers();
        //    });
        //}
    }
}
