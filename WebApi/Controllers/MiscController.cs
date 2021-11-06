using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace WebApi.Controllers
{
    
    [ApiController]
    [Route("api/misc")]
    public class MiscController : WebApiController
    {

        public MiscController(IHostEnvironment environment) : base(environment)
        {
          
        }

        [HttpGet]
        [Route("hello-world")]
        public ActionResult HelloWorld()
        {
            return Ok();
        }

    }
}
