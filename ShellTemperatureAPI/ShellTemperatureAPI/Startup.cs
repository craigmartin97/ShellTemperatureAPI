using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShellTemperature.Data;
using ShellTemperature.Repository;
using ShellTemperature.Repository.Interfaces;

namespace ShellTemperatureAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // setup inject db
            services.AddDbContext<ShellDb>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ShellConnection")
                        , optionsBuilder =>
                        {
                            optionsBuilder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
                        }),
                ServiceLifetime.Transient);

            services.AddScoped<IRepository<ShellTemp>, ShellTemperatureRepository>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
