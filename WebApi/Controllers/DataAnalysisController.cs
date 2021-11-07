using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Services.Dtos;
using Services.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/data-analysis")]
    public class DataAnalysisController : WebApiController
    {

        private readonly IDataAnalysisService _dataAnalysisService;

        public DataAnalysisController(IHostEnvironment environment, IDataAnalysisService dataAnalysisService) : base(environment) 
        {
            _dataAnalysisService = dataAnalysisService;
        }

        [HttpPost("analyze")]
        public async Task<ActionResult<MatchingMetricsDto>> ImportData([FromBody] PostForMetricsDto request)
        {
            var result = await _dataAnalysisService.ImportPostInformationAsync(request);
            return Ok(result);
        }

    }
}
