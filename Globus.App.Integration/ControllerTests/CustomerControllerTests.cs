using Globus.App.DTO;
using Globus.App.Integration.Tests.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Globus.App.Integration.Tests.ControllerTests
{
    public class CustomerControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;

        public CustomerControllerTests(WebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/customer/");
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task get_all_customers_returns_non_empty_list_of_customers()
        {
            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddDbContext<GlobusContext>(config =>
                    {
                        config.UseInMemoryDatabase("GlobusDB");
                    },ServiceLifetime.Singleton);
                    
                    var context = services.BuildServiceProvider().GetRequiredService<GlobusContext>();
                    context.Customer.Add(new Data.Entities.Customer
                    {
                        EmailAddress = "me@damilola.io",
                        PhoneNumber = "08161565015",
                        LGA = "Agege",
                        State = "Lagos",
                        CreationDateTime = DateTime.Now,
                        HashedPassword = Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("@Password123")))
                    });
                    context.SaveChanges();
                });

            });

            var client = factory.CreateClient();

            var model = await client.GetFromJsonAsync<List<CustomerResponse>>("");
            Assert.NotNull(model);
        }

        [Fact]
        public async Task customer_creation_without_emailaddress_should_fail()
        {
            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddDbContext<GlobusContext>(config =>
                    {
                        config.UseInMemoryDatabase("GlobusDB");
                    }, ServiceLifetime.Singleton);
                });

            });

            var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync<CreateCustomerRequest>("",
                new CreateCustomerRequest
                {
                     PhoneNumber = "08161565015",
                     LGA = "Agege",
                     Password = "@Password123",
                     State = "Lagos"
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task customer_creation_without_phonenumber_should_fail()
        {
            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddDbContext<GlobusContext>(config =>
                    {
                        config.UseInMemoryDatabase("GlobusDB");
                    }, ServiceLifetime.Singleton);
                });

            });

            var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync<CreateCustomerRequest>("",
                new CreateCustomerRequest
                {
                    EmailAddress = "me@damilola.io",
                    LGA = "Agege",
                    Password = "@Password123",
                    State = "Lagos"
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task customer_creation_without_password_should_fail()
        {
            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddDbContext<GlobusContext>(config =>
                    {
                        config.UseInMemoryDatabase("GlobusDB");
                    }, ServiceLifetime.Singleton);
                });

            });

            var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync<CreateCustomerRequest>("",
                new CreateCustomerRequest
                {
                    EmailAddress = "me@damilola.io",
                    LGA = "Agege",
                    PhoneNumber = "08161565015",
                    State = "Lagos"
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task customer_creation_without_state_should_fail()
        {
            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddDbContext<GlobusContext>(config =>
                    {
                        config.UseInMemoryDatabase("GlobusDB");
                    }, ServiceLifetime.Singleton);
                });

            });

            var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync<CreateCustomerRequest>("",
                new CreateCustomerRequest
                {
                    EmailAddress = "me@damilola.io",
                    LGA = "Agege",
                    PhoneNumber = "08161565015",
                    Password = "@Password123"
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task customer_creation_without_lga_should_fail()
        {
            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddDbContext<GlobusContext>(config =>
                    {
                        config.UseInMemoryDatabase("GlobusDB");
                    }, ServiceLifetime.Singleton);
                });

            });

            var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync<CreateCustomerRequest>("",
                new CreateCustomerRequest
                {
                    EmailAddress = "me@damilola.io",
                    PhoneNumber = "08161565015",
                    Password = "@Password123",
                    State = "Lagos"
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task customer_creation_without_otp_validation_should_fail()
        {
            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddDbContext<GlobusContext>(config =>
                    {
                        config.UseInMemoryDatabase("GlobusDB");
                    }, ServiceLifetime.Singleton);
                });

            });

            var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync<CreateCustomerRequest>("",
                new CreateCustomerRequest
                {
                    EmailAddress = "me@damilola.io",
                    PhoneNumber = "08161565015",
                    Password = "@Password123",
                    LGA = "Agege",
                    State = "Lagos"
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
