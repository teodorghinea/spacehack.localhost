using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Services.Dtos;
using Services.Services.Facebook;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/facebook")]
    public class FacebookController : WebApiController
    {
        private readonly IFacebookService _facebookService;

        public FacebookController(IHostEnvironment environment, IFacebookService facebookService) : base(environment)
        {
            _facebookService = facebookService;
        }


        [HttpGet("posts")]
        public async Task<ActionResult<List<FacebookPostDto>>> GetPosts([FromQuery] int skip = 0, int take = 0)
        {
            var result = await _facebookService.GetPostsAsync(skip, take);
            return Ok(result);
        }
    }
}
