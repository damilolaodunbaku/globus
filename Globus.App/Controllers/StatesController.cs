using Globus.App.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Globus.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public ActionResult<List<string>> GetStates()
        {
            return new JsonResult(_nigerianStatesService.GetAllStates());
        }

        [HttpGet("lga")]
        public ActionResult GetLgasForState(string state)
        {
            return new JsonResult(_nigerianStatesService.GetLgaForState(state));
        }
    }
}
