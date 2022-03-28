using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Globus.App.Integration.ControllerTests
{
    public class StateControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public StateControllerTest(WebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/states/");
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task get_all_states_should_not_be_empty()
        {
            var model = await _client.GetFromJsonAsync<List<string>>("");
            Assert.NotNull(model);
            Assert.NotEmpty(model);
        }

        [Fact]
        public async Task get_lgas_for_valid_state_should_not_be_empty()
        {
            var model = await _client.GetFromJsonAsync<List<string>>($"lga?State=lagos");
            Assert.NotNull(model);
            Assert.NotEmpty(model);
        }

        [Fact]
        public async Task get_lgas_for_invalid_state_should_be_empty()
        {
            var model = await _client.GetFromJsonAsync<List<string>>($"lga?State=Alaska");
            Assert.NotNull(model);
            Assert.Empty(model);
        }
    }
}
