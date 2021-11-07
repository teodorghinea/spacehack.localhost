using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Services.Services.DatabaseParser;
using System.Threading.Tasks;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("api/do-not-touch-this/database-seeder")]
    public class DatasetController : WebApiController
    {
        private readonly IDatabaseSeederService _seeder;

        public DatasetController(IHostEnvironment environment, IDatabaseSeederService seeder) : base(environment)
        {
            _seeder = seeder;
        }

        [HttpGet]
        [Route("seed/facebook")]
        public async Task<ActionResult<string>> FacebookDataSeed()
        {
            await _seeder.GetContentAsync(typeof(FacebookPost), "HOOTSUITE_FACEBOOKPAGE.tsv");
            return Ok();
        }

        [HttpGet]
        [Route("seed/facebook/key-words")]
        public async Task<ActionResult<bool>> FacebookDataKeywordSeed()
        {
            var result = await _seeder.Facebook_KeywordseedAsync();
            return Ok(result);
        }

    }
}
