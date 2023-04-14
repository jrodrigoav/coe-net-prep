using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [Route("api/time"), ApiController, Authorize]
    public class TimeController : ControllerBase
    {
        [HttpGet]
        public string Get() => DateTime.Now.ToString("s");
    }
}
