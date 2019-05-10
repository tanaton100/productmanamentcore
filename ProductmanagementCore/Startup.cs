using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductmanagementCore.Repository;
using ProductmanagementCore.Services;
using Swashbuckle.AspNetCore.Swagger;

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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //defaut dependency injection ทุกๆคลาสที่implement ต้องเซตauto injectionทุกคลาส
            //services.AddTransient<IUserRepository, UserRepository>();
            //services.AddTransient<IUserService, UserService>();
            //services.AddTransient<IOrdersRepository, OrdersRepository>();
            //services.AddTransient<IProductRepository, ProductRepository>();

            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "ProductManagement", Version = "v1"}); });

            var builder = new ContainerBuilder();

            var serviceAssembly = typeof(UserService).Assembly;
            builder.RegisterAssemblyTypes(serviceAssembly)
                .Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().SingleInstance();

            var repositoryAssembly = typeof(UserRepository).Assembly;
            builder.RegisterAssemblyTypes(repositoryAssembly)
                .Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().SingleInstance();

            builder.Populate(services);
            var contriner = builder.Build();


            return new AutofacServiceProvider(contriner);
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

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductManagement"); });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
        }
    }
}
