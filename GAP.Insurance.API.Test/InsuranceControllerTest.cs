using AutoMapper;
using FluentAssertions;
using GAP.Insurance.API.Controllers;
using GAP.Insurance.Common.Attributes;
using GAP.Insurance.Common.Exceptions;
using GAP.Insurance.Common.Infrastructure;
using GAP.Insurance.Core.InsuranceModule;
using GAP.Insurance.Domain;
using GAP.Insurance.TO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GAP.Insurance.API.Test
{
    public class InsuranceControllerTest
    {
        private readonly IInsuranceRepository _insuranceRepository;

        public InsuranceControllerTest()
        {
            var services = new ServiceCollection();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(CustomActionFilterAttribute));
                options.Filters.Add(typeof(CustomExceptionFilterAttribute));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //The context options could be singleton, not the context instance because each request has a scoped lifetime - atomic transactions
            var contextOptions = new DbContextOptionsBuilder<DBInsuranceContext>().UseSqlServer("Server=tcp:jcm-group.database.windows.net,1433;Initial Catalog=DBInsurance;Persist Security Info=False;User ID=USR_INSURANCE;Password=IN$_2019;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;").Options;
            services.AddSingleton(contextOptions);

            Assembly currentExecutingAssembly = Assembly.GetAssembly(typeof(InsuranceController));
            services.AddSingleton<ILocalizationService>(new LocalizationService(currentExecutingAssembly.GetName().Name + ".Resources.Messages", currentExecutingAssembly));
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddTransient<IInsuranceRepository, InsuranceRepository>();

            //Mapping configuration is singleton too
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Insurance.Domain.Insurance, InsuranceTO>();
                cfg.CreateMap<Insurance.Domain.Customer, InsuranceTO>();
                cfg.CreateMap<Insurance.Domain.CustomerInsurance, CustomerInsuranceTO>();
                cfg.CreateMap<InsuranceTO, Insurance.Domain.Insurance>();
                cfg.CreateMap<InsuranceTO, Insurance.Domain.Customer>();
                cfg.CreateMap<CustomerInsuranceTO, Insurance.Domain.CustomerInsurance>();
            });
            services.AddSingleton(mapperConfiguration);

            var serviceProvider = services.BuildServiceProvider();
            _insuranceRepository = serviceProvider.GetService<IInsuranceRepository>();
        }

        #region Add New Customer  

        [Fact]
        public async Task Task_Add_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new InsuranceController(_insuranceRepository);
            InsuranceTO insurance = new InsuranceTO()
            {
                Name = "Insurance Test 01",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1),
                CoverageType = "Earthquake",
                Coverage = 45,
                Cost = 123,
                RiskType = "Low"
            };

            //Act  
            var data = await controller.Save(insurance);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async Task Task_Add_RiskTypeHigh_Coverage_GreaterThan50()
        {
            //Arrange  
            var controller = new InsuranceController(_insuranceRepository);
            InsuranceTO insurance = new InsuranceTO()
            {
                Name = "Insurance Test 02",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1),
                CoverageType = "Earthquake",
                Coverage = 90,
                Cost = 123,
                RiskType = "High"
            };

            //Act
            Exception ex = await Assert.ThrowsAsync<CustomException>(() => controller.Save(insurance));

            //Assert  
            Assert.Equal("Coverage can't be greater than 50 when the insurance has a high risk type.", ex.Message);
        }

        [Fact]
        public async void Task_Add_ValidData_MatchResult()
        {
            //Arrange  
            var controller = new InsuranceController(_insuranceRepository);
            InsuranceTO insurance = await GetInsurance();
            insurance.Description = "Insurance Description";

            //Act  
            var data = await controller.Save(insurance);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<InsuranceTO>().Subject;

            Assert.Equal(insurance.InsuranceId, result.InsuranceId);
            Assert.Equal(insurance.Name, result.Name);
            Assert.Equal(insurance.Description, result.Description);
        }

        #endregion

        #region Private methods
        private async Task<InsuranceTO> GetInsurance()
        {
            var list = await _insuranceRepository.GetAll();
            return list?.FirstOrDefault();
        }
        #endregion
    }
}
