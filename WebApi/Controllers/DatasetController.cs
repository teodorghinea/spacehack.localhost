using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Services.Services.DatabaseParser;
using System.Threading.Tasks;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("api/database-seeder")]
    public class DatasetController : WebApiController
    {
        private readonly IDatabaseSeederService _seeder;

        public DatasetController(IHostEnvironment environment, IDatabaseSeederService seeder) : base(environment)
        {
            _seeder = seeder;
        }

        [HttpGet]
        [Route("seed/facebook")]
        public async Task<ActionResult<string>> HelloWorld()
        {
            await _seeder.GetContentAsync(typeof(FacebookPost), "HOOTSUITE_FACEBOOKPAGE.tsv");
            return Ok();
        }

    }
}
