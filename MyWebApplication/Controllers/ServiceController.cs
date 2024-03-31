using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MyWebApplication.Services;
using System.Threading.Tasks;

namespace MyWebApplication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly MyBackgroundService _backgroundService;

        public ServiceController(MyBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartService()
        {
            return Ok( _backgroundService.StartService());
        }

        [HttpPost("stop")]
        public async Task<IActionResult> StopService()
        {
            return Ok( _backgroundService.StopService());
        }
    }

}
