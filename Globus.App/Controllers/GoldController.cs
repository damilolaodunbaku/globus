using Globus.App.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Globus.App.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
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
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation("Get price of Gold and Silver", "Get price of Gold and Silver")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the price of Gold and Silver for today", Type = typeof(PricesResponse))]

        public async Task<ActionResult<PricesResponse>> GetPricesAsync()
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
