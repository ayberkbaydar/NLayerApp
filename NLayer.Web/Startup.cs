using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLayer.Repository;
using NLayer.Service.Mappings;
using NLayer.Service.Validations;
using NLayer.Web.Services;
using System;
using System.Reflection;

namespace NLayer.Web
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
            services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>()); ;

            services.AddAutoMapper(typeof(MapProfile));

            services.AddDbContext<AppDbContext>(x =>
            {
                x.UseSqlServer(Configuration.GetConnectionString("SqlConnection"), option =>
                {
                    option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
                });
            });

            services.AddHttpClient<ProductApiService>(opt =>
            {
                //opt.BaseAddress = new Uri(Configuration.GetSection("BaseUrl").Value);
                opt.BaseAddress = new Uri(Configuration["BaseUrl"]);
                //opt.BaseAddress = new Uri("https://localhost:7264/api/");
            });
            services.AddHttpClient<CategoryApiService>(opt =>
            {
                //opt.BaseAddress = new Uri(Configuration.GetSection("BaseUrl").Value);
                opt.BaseAddress = new Uri(Configuration["BaseUrl"]);
                //opt.BaseAddress = new Uri("https://localhost:7264/api/");
            });

            services.AddScoped(typeof(NotFoundFilter<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
