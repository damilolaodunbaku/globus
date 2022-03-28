using Globus.App.Business.Services;
using Globus.App.Data;
using Globus.App.Data.Contexts;
using Globus.App.Data.Repositories;
using Globus.App.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Globus.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", 
                    new OpenApiInfo { 
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Email = "me@damilola.io",
                        Name = "Damilola Odunbaku",

                    },
                    Description = "A set of API'S for Globus Developer Test.",
                    Title = "Globus.App",
                    Version = "v1" 
                });
                setupAction.EnableAnnotations();
            });

            services.AddDbContext<GlobusContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"),
                    sqlServerOptions =>
                    {
                        sqlServerOptions.MigrationsAssembly("Globus.App.Data");
                    });
            });

            services.AddHttpClient("goldApiClient", config =>
            {
                config.DefaultRequestHeaders.TryAddWithoutValidation("X-RapidAPI-Host", Configuration["goldUrlHost"]);
                config.DefaultRequestHeaders.TryAddWithoutValidation("X-RapidAPI-Key", Configuration["goldApiKey"]);
            });

            services.AddScoped<OTPRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<NigerianStatesService>();
            services.AddSingleton<NotificationService>();
            services.AddSingleton<EncryptionService>();
            services.AddSingleton<MnoCodeService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Globus.App v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //using (var scope = app.ApplicationServices.CreateScope())
            //{
            //    using (var context = scope.ServiceProvider.GetService<GlobusContext>())
            //    {
            //        context.Database.Migrate();
            //    }
            //}
        }
    }
}
