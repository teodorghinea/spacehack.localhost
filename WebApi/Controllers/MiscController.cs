﻿using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Services.Services.DatabaseParser;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class MiscController : WebApiController
    {
        private readonly IDatabaseSeederService _seeder;

        public MiscController(IHostEnvironment environment, IDatabaseSeederService seeder) : base(environment) 
        {
            _seeder = seeder;
        }

        [HttpGet]
        [Route("hello-world")]
        public async Task<ActionResult<string>> HelloWorld()
        {
            await _seeder.GetContentAsync(typeof(FacebookPost), "HOOTSUITE_FACEBOOKPAGE.csv");
            return Ok("Hello World!");
        }

    }
}
