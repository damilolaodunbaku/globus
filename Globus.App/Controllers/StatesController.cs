using Globus.App.Business.Services;
using Globus.App.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace Globus.App.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public class StatesController : ControllerBase
    {
        private readonly ILogger<StatesController> _logger;
        private readonly NigerianStatesService _nigerianStatesService;

        public StatesController(ILogger<StatesController> logger,
            NigerianStatesService nigerianStatesService)
        {
            _logger = logger;
            _nigerianStatesService = nigerianStatesService;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation("Get all states", "Get all Nigerian states")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of all Nigerian states", Type = typeof(List<string>), ContentTypes = new string[] { MediaTypeNames.Application.Json })]
        public ActionResult<List<string>> GetStates()
        {
            try
            {
                return new JsonResult(_nigerianStatesService.GetAllStates());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("lga")]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerOperation("Get all LGA's in a state", "Get all Local government areas in a state")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of all LGA's in the provided state", Type = typeof(List<string>), ContentTypes = new string[] { MediaTypeNames.Application.Json })]
        public ActionResult<List<string>> GetLgasForState([FromQuery]LgaRequest request)
        {
            try
            {
                return new JsonResult(_nigerianStatesService.GetLgaForState(request.State));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
