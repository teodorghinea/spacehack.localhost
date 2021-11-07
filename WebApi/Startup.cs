using System.Collections.Generic;
using System.Net;
using AutoMapper;
using DataLayer;
using DataLayer.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Services.AutomapperProfiles;
using Services.Services;
using Services.Services.DatabaseParser;
using Services.Services.Facebook;
using Services.Services.MonkeyLearnService;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EfDbContext>(options => options
                    .UseSqlServer(Configuration.GetConnectionString("localhostTeam")));

            AddAutoMapper(services);
            AddDependencies(services);
            services.AddControllers();
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "localhost", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        if (contextFeature.Error is BadRequestException)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        }
                        else
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        }

                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new List<string>
                            {contextFeature.Error.Message}));
                    }
                });
            });


            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "localhost.SpaceHack");
                c.RoutePrefix = "api/swagger";
            });


            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddDependencies(IServiceCollection services)
        {
            AddServices(services);
            AddRepositories(services);
        }

        private void AddServices(IServiceCollection services)
        {
            services.AddScoped<IDatabaseSeederService, DatabaseSeederService>();
            services.AddScoped<IFacebookService, FacebookService>();
            services.AddScoped<IDataAnalysisService, DataAnalysisService>();
            services.AddScoped<ICompetitorService, CompetitorService>();
            services.AddScoped<IRequestService, RequestService>();
        }

        private void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFacebookPostRepository, FacebookPostRepository>();
            services.AddScoped<ICompetitorRepository, CompetitorRepository>();
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfiles());
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
