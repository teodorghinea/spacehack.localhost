using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Services.Dtos;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("api/competitors")]
    public class CompetitorController : WebApiController
    {
        private readonly ICompetitorService _competitorsService;

        public CompetitorController(IHostEnvironment environment, ICompetitorService competitorService) : base(environment)
        {
            _competitorsService = competitorService;
        }

        [HttpGet("my-page")]
        public async Task<ActionResult<CompetitorDto>> GetMyAccount()
        {
            var result = await _competitorsService.GetMyAccount();
            return Ok(result);
        }

        [HttpGet("by-id/{id}")]
        public async Task<ActionResult<CompetitorDto>> GetCompetitorById([FromRoute] Guid id)
        {
            var result = await _competitorsService.GetCompetitorByIdAsync(id);
            return result;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<CompetitorDto>>> GetAllCompetitors()
        {
            var result = await _competitorsService.GetAllCompetitorsAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddCompetitor([FromBody] CompetitorDto competitor)
        {
            var result = await _competitorsService.AddNewCompetitorAsync(competitor);
            return Ok(result);
        }

    }
}
