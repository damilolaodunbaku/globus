using Globus.App.DTO;
using Globus.App.Integration.Tests.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    services.AddDbContext<GlobusTestContext>(config =>
                    {
                        config.UseInMemoryDatabase("GlobusDB");
                    },ServiceLifetime.Singleton);
                });

            });

            var client = factory.CreateClient();

            var context = factory.Services.GetRequiredService<GlobusTestContext>();
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

            var model = await client.GetFromJsonAsync<List<CustomerResponse>>("");
            Assert.NotNull(model);
            Assert.NotEmpty(model);
        }
    }
}
