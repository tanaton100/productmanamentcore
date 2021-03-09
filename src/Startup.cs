using System.Reflection;
using Autofac;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddSwaggerGen();
            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        }

    

        public void ConfigureContainer(ContainerBuilder builder)
        {

                 var serviceAssembly = typeof(UserService).Assembly;
            builder.RegisterAssemblyTypes(serviceAssembly)
                .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().SingleInstance();

            var repositoryAssembly = typeof(UserRepository).Assembly;
            builder.RegisterAssemblyTypes(repositoryAssembly)
                .Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().SingleInstance();
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
            });

        }
    }
}
