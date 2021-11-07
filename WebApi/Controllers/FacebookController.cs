using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Services.Dtos;
using Services.Services.Facebook;
using System;
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

        [HttpGet("posts/by-id/{id}")]
        public async Task<ActionResult<FacebookPostDto>> GetById([FromRoute] Guid id)
        {
            var result = await _facebookService.GetByIdAsync(id);
            return result;
        }

        [HttpGet("posts")]
        public async Task<ActionResult<List<LightFacebookPostDto>>> GetPosts([FromQuery] int skip = 0, int take = 0)
        {
            var result = await _facebookService.GetPostsAsync(skip, take);
            return Ok(result);
        }

        [HttpPost("posts")]
        public async Task<bool> PostData([FromBody] List<FacebookPostAddDto> postData)
        {
            var result = await _facebookService.AddPostAsync(postData);
            return result;
        }

    }
}
