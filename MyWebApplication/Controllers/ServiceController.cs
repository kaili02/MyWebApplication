using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MyWebApplication.Services;
using MyWebApplication.Util;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MyWebApplication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly MyBackgroundService _backgroundService;
        private static readonly ILogger<ServiceController> _logger = ServiceUtil.Logger<ServiceController>();

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

        [HttpPost("Post")]
        public ActionResult Post([FromBody] YourModel model)
        {
            _logger.LogInformation("model:{}", model);
            return Ok();
        }
    }

    public class YourModel
    {
        [Required]
        public string type { get; set; }
    
        [Range(0, 100)]
        public int Parameter2 { get; set; }
        
        [MaxLength(10, ErrorMessage = "List must not exceed 10 items.")]
        [MinLength(1, ErrorMessage = "List must have at least 1 item.")]
        public List<string> Items { get; set; }

        [MaxLength(10, ErrorMessage = "List must not exceed 10 items.")]
        [MinLength(0, ErrorMessage = "List must have at least 1 item.")]
        public List<string> Items2 { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Parameter1 can only contain letters and spaces.")]
        public string Parameter1 { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Parameter1 length must be between 3 and 50.")]
        public string Parameter3 { get; set; }



    }

}
