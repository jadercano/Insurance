using GAP.Insurance.TO;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GAP.Insurance.API.Test.Integration
{
    /// <summary>
    /// Implement integration test for CustomerController
    /// </summary>
    public class CustomerTest : IClassFixture<WebApplicationFactory<GAP.Insurance.API.Startup>>
    {
        private readonly WebApplicationFactory<GAP.Insurance.API.Startup> _factory;
        private List<CustomerTO> _customers;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly JsonMediaTypeFormatter _jsonMediaTypeFormatter;

        public CustomerTest(WebApplicationFactory<GAP.Insurance.API.Startup> factory)
        {
            _factory = factory;

            _customers = new List<CustomerTO> {
                new CustomerTO {
                    Name = "IntegrationTest01",
                    Email = "integrationtest01@test.com"
                },
                new CustomerTO {
                    Name = "IntegrationTest02",
                    Email = "integrationtest02@test.com"
                },
                new CustomerTO {
                    Name = "IntegrationTest03",
                    Email = "integrationtest03@test.com"
                }
            };

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            _jsonMediaTypeFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = _jsonSerializerSettings
            };
        }

        [Theory]
        [InlineData("/api/Customer")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Post_CreateCustomer(int index)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/Customer", _customers[index], _jsonMediaTypeFormatter, "application/json");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<CustomerTO>(content, _jsonSerializerSettings);

            Assert.NotNull(customer);
            Assert.NotEqual(customer.CustomerId, Guid.Empty);
            Assert.Equal(customer.Name, _customers[index].Name);
            Assert.Equal(customer.Email, _customers[index].Email);
        }

        [Fact]
        public async Task Get_GetAll()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Customer");

            // Assert
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            _customers = JsonConvert.DeserializeObject<List<CustomerTO>>(content, _jsonSerializerSettings);

            Assert.NotEmpty(_customers);
        }
    }
}
