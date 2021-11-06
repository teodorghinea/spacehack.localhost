using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class WebApiController : Controller
    {
        protected readonly IHostEnvironment Environment;

        public WebApiController(IHostEnvironment environment)
        {
            Environment = environment;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            CheckModel(context);
            return base.OnActionExecutionAsync(context, next);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            CheckModel(context);
            base.OnActionExecuting(context);
        }

        [NonAction]
        private void CheckModel(ActionExecutingContext context)
        {
            if (!ModelState.IsValid)
            {
                context.Result = BadRequest(
                    ModelState
                        .Values
                        .SelectMany(x => x.Errors
                            .Select(y => y.ErrorMessage))
                        .ToList()
                );
            }
        }

    }
}
