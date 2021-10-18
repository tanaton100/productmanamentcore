using System;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using Autofac;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProductmanagementCore.Common;
using ProductmanagementCore.Repository;
using ProductmanagementCore.Services;

namespace ProductmanagementCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddHealthChecks()
            .AddSqlServer(Configuration.GetConnectionString("DefaultConnection"))
             .AddUrlGroup(new Uri("https://localhost:5001/api/Order/getallt"), "Order");
            services.AddSwaggerGen();
            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
     
         }

    

        public void ConfigureContainer(ContainerBuilder builder)
        {

                 var serviceAssembly = typeof(UserService).Assembly;
            builder.RegisterAssemblyTypes(serviceAssembly)
                .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerDependency();

            var repositoryAssembly = typeof(UserRepository).Assembly;
            builder.RegisterAssemblyTypes(repositoryAssembly)
                .Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerDependency();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductManagement");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthcheck");
                endpoints.MapHealthChecks("/healthcheck-details",
             new HealthCheckOptions
             {
                 ResponseWriter = async (context, report) =>
                 {
                     var result = JsonSerializer.Serialize(
                         new
                         {
                             status = report.Status.ToString(),
                             monitors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
                         });
                     context.Response.ContentType = MediaTypeNames.Application.Json;
                     await context.Response.WriteAsync(result);
                 }
             });
            });

        }
    }
}
