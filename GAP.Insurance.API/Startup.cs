using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using GAP.Insurance.Common.Attributes;
using GAP.Insurance.Common.Helpers;
using GAP.Insurance.Common.Infrastructure;
using GAP.Insurance.Core.CustomerModule;
using GAP.Insurance.Core.InsuranceModule;
using GAP.Insurance.Domain;
using GAP.Insurance.TO;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

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
                                options.EnableEndpointRouting = false;
                options.Filters.Add(typeof(CustomActionFilterAttribute));
                options.Filters.Add(typeof(CustomExceptionFilterAttribute));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // Enable CORS
            var allowedOrigins = new List<string>();
            Configuration.GetSection("AllowedOrigins").Bind(allowedOrigins);
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(allowedOrigins.ToArray())
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            // Register application services.
            Assembly currentExecutingAssembly = Assembly.GetExecutingAssembly();
            services.AddSingleton<ILocalizationService>(new LocalizationService(currentExecutingAssembly.GetName().Name + ".Resources.Messages", currentExecutingAssembly));
            services.AddSingleton<ILoggerService, LoggerService>();

            //The context options could be singleton, not the context instance because each request has a scoped lifetime - atomic transactions
            var contextOptions = new DbContextOptionsBuilder<DBInsuranceContext>().UseSqlServer(Configuration.GetConnectionString("InsuranceDatabase")).Options;
            services.AddSingleton(contextOptions);
            services.AddSingleton(typeof(ICustomerRepository), typeof(CustomerRepository));
            services.AddSingleton(typeof(IInsuranceRepository), typeof(InsuranceRepository));

            //Mapping configuration is singleton too
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Insurance.Domain.Insurance, InsuranceTO>().ReverseMap();
                cfg.CreateMap<Insurance.Domain.Customer, CustomerTO>()
                    .ForMember(d => d.Insurances, opt => opt.MapFrom(src => src.CustomerInsurance))
                    .ReverseMap();
                cfg.CreateMap<Insurance.Domain.CustomerInsurance, CustomerInsuranceTO>()
                    .ForMember(d => d.Insurance, opt => opt.MapFrom(src => src.Insurance))
                    .ReverseMap();
            });
            services.AddSingleton(mapperConfiguration);

            // Register the Swagger Generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Insurance.API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerService logger)
        {
            InitializeLog4Net(logger);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "Insurance.API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        /// <summary>
        /// Allows to initialize log4net
        /// </summary>
        /// <param name="logger">The logger to be used</param>
        private void InitializeLog4Net(ILoggerService logger)
        {
            var log4NetConfig = new XmlDocument();
            log4NetConfig.Load(File.OpenRead("log4net.config"));

            ILoggerRepository repo = log4net.LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4NetConfig["log4net"]);

            logger.WriteLog(LogCategory.Debug, "INFO_Log4NetStarted", true, null, null);
        }
    }
}
