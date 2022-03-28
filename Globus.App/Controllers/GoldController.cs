using Globus.App.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Globus.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoldController : ControllerBase
    {
        private readonly ILogger<GoldController> _logger;
        private readonly HttpClient _goldApiClient;
        private readonly string _goldPricesUrl;
        public GoldController(ILogger<GoldController> logger,
            IHttpClientFactory clientFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _goldPricesUrl = configuration["goldPricesUrl"];
            _goldApiClient = clientFactory.CreateClient("goldApiClient");
        }

        [HttpGet]
        public async Task<ActionResult> GetPricesAsync()
        {
            try
            {
                using (HttpResponseMessage responseMessage =
                    await _goldApiClient.GetAsync(_goldPricesUrl))
                {
                    responseMessage.EnsureSuccessStatusCode();
                    PricesResponse responseBody = 
                          JsonConvert.DeserializeObject<PricesResponse>
                        (await responseMessage.Content.ReadAsStringAsync());
                    return new JsonResult(responseBody);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
