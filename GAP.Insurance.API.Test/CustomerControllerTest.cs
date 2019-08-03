using AutoMapper;
using FluentAssertions;
using GAP.Insurance.API.Controllers;
using GAP.Insurance.Common.Infrastructure;
using GAP.Insurance.Core.CustomerModule;
using GAP.Insurance.Domain;
using GAP.Insurance.TO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GAP.Insurance.API.Test
{
    public class CustomerControllerTest
    {
        private readonly ICustomerRepository _customerRepository;

        private CustomerTO _customer { get; set; }

        public CustomerControllerTest()
        {
            var services = new ServiceCollection();

            //The context options could be singleton, not the context instance because each request has a scoped lifetime - atomic transactions
            var contextOptions = new DbContextOptionsBuilder<DBInsuranceContext>().UseSqlServer("Server=tcp:jcm-group.database.windows.net,1433;Initial Catalog=DBInsurance;Persist Security Info=False;User ID=USR_INSURANCE;Password=IN$_2019;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;").Options;
            services.AddSingleton(contextOptions);

            Assembly currentExecutingAssembly = Assembly.GetAssembly(typeof(CustomerController));
            services.AddSingleton<ILocalizationService>(new LocalizationService(currentExecutingAssembly.GetName().Name + ".Resources.Messages", currentExecutingAssembly));
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();

            //Mapping configuration is singleton too
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Insurance.Domain.Insurance, InsuranceTO>();
                cfg.CreateMap<Insurance.Domain.Customer, CustomerTO>();
                cfg.CreateMap<Insurance.Domain.CustomerInsurance, CustomerInsuranceTO>();
                cfg.CreateMap<InsuranceTO, Insurance.Domain.Insurance>();
                cfg.CreateMap<CustomerTO, Insurance.Domain.Customer>();
                cfg.CreateMap<CustomerInsuranceTO, Insurance.Domain.CustomerInsurance>();
            });
            services.AddSingleton(mapperConfiguration);

            var serviceProvider = services.BuildServiceProvider();
            _customerRepository = serviceProvider.GetService<ICustomerRepository>();
        }

        #region Add New Customer  

        [Fact]
        public async void Task_Add_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new CustomerController(_customerRepository);
            CustomerTO customer = new CustomerTO()
            {
                Name = "Customer Test",
                Email = "customer@test.com"
            };

            //Act  
            var data = await controller.Save(customer);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async Task Task_Add_InvalidData_Return_BadRequest()
        {
            //Arrange  
            var controller = new CustomerController(_customerRepository);
            CustomerTO customer = new CustomerTO()
            {
                Name = "Test Customer Name With More Than 50 Characteres for testing a bad request",
                Email = "Test Email"
            };

            //Act              
            var data = await controller.Save(customer);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_Add_ValidData_MatchResult()
        {
            //Arrange  
            var controller = new CustomerController(_customerRepository);
            CustomerTO customer = new CustomerTO()
            {
                Name = "Another Customer Test",
                Email = "anothercustomer@test.com"
            };

            //Act  
            var data = await controller.Save(customer);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<CustomerTO>().Subject;

            Assert.Equal(customer.CustomerId, result.CustomerId);
            Assert.Equal(customer.Name, result.Name);
            Assert.Equal(customer.Email, result.Email);

            //Set customer for other test
            _customer = result;
        }

        #endregion

        #region GetById  

        [Fact]
        public async void Task_GetById_Return_OkResult()
        {
            //Arrange  
            var controller = new CustomerController(_customerRepository);
            var customerId = _customer.CustomerId;

            //Act  
            var data = await controller.GetById(customerId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetById_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new CustomerController(_customerRepository);
            var customerId = Guid.NewGuid();

            //Act  
            var data = await controller.GetById(customerId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_GetById_MatchResult()
        {
            //Arrange  
            var controller = new CustomerController(_customerRepository);
            Guid customerId = _customer.CustomerId;

            //Act  
            var data = await controller.GetById(customerId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var customer = okResult.Value.Should().BeAssignableTo<CustomerTO>().Subject;

            Assert.Equal(_customer.CustomerId, customer.CustomerId);
            Assert.Equal(_customer.Name, customer.Name);
            Assert.Equal(_customer.Email, customer.Email);
        }

        #endregion
    }
}
