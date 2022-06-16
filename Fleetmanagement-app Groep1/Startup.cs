using Fleetmanagement_app_BLL.UnitOfWork;
using Fleetmanagement_app_DAL.Database;
using FleetManagement_app_PL.Profiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Fleetmanagement_app_Groep1
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


            services.AddDbContext<FleetmanagerContext>(options =>
            {
                //Ophalen van connectionstring uit database
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            // Deze service zorgt ervoor dat cors errors vermeden worden als front- en back-end locale url's hebben, als de server
            // een andere url dan de lokale heeft dient deze toegevoegd te worden in appsettings.json als value bij de key "UrlToApi".
            services.AddCors(options =>
            {
                string url = Configuration.GetSection("UrlToApi").Value;

                //if (url == "")
                //{
                //    options.AddDefaultPolicy(builder =>
                //        builder.SetIsOriginAllowed(o => new Uri(o).Host == "localhost")
                //            .AllowAnyHeader()
                //            .AllowAnyMethod());
                //}
                //else

                    options.AddDefaultPolicy(builder => builder.WithOrigins(url)
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod());
            });


            services.AddScoped<IUnitOfWork, UnitOfWork>()
                .AddSingleton<ILoggerFactory, LoggerFactory>();

            services.AddAutoMapper(typeof(VoertuigProfile));
            services.AddAutoMapper(typeof(BestuurderProfile));
            services.AddAutoMapper(typeof(TankkaartProfile));

            services.AddControllersWithViews(o => o.EnableEndpointRouting = true);
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
 
                //app.Use(async (context, next) =>
                //{
                //    context.Response.OnStarting(() =>
                //    {
                //        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                //        return Task.FromResult(0);
                //    });

                //    await next();
                //});


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}