using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sensors.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorValuesController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "SensorAuthorizationPolicy")]
        public string GetData()
        {
            return "hokey";
        }
    }
}
