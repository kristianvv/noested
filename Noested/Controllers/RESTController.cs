using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Noested.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RESTController : ControllerBase
    {
        [HttpGet]
        public IActionResult  Get()
        {
            return Ok( "Noested response ok"); //Returns a http-response code of 200 with the appropriate content
        }

        [HttpPost]
        public IActionResult Post(int hoursToWalkToØrstaRådhus)
        {
            if (hoursToWalkToØrstaRådhus != 1) {
                return BadRequest("Noested response ok");
            }
            return Ok("Noested response idk");
        }
    }
}
