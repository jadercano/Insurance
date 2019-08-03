using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GAP.Insurance.Common.Attributes;
using GAP.Insurance.Common.Infrastructure;
using GAP.Insurance.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GAP.Insurance.API
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
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(CustomActionFilterAttribute));
                options.Filters.Add(typeof(CustomExceptionFilterAttribute));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register application services.
            Assembly currentExecutingAssembly = Assembly.GetExecutingAssembly();
            services.AddSingleton<ILocalizationService>(new LocalizationService(currentExecutingAssembly.GetName().Name + ".Resources.Messages", currentExecutingAssembly));
            services.AddSingleton<ILoggerService, LoggerService>();

            //The context options could be singleton, not the context instance because each request has a scoped lifetime - atomic transactions
            var contextOptions = new DbContextOptionsBuilder<DBInsuranceContext>().UseSqlServer(Configuration.GetConnectionString("InsuranceDatabase")).Options;
            services.AddSingleton(contextOptions);
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
