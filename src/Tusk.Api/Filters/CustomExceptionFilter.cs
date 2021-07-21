using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Serilog;
using Tusk.Api.Exceptions;

namespace Tusk.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment? _env;

        public CustomExceptionFilter(
            IWebHostEnvironment env) =>
            _env = env;

        [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        public override void OnException(
            ExceptionContext context)
        {
            var code = context.Exception switch
            {
                InvalidOperationException _ => HttpStatusCode.BadRequest,
                NotFoundException _ => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };

            if (code == HttpStatusCode.InternalServerError)
            {
                Log.Error("The interanal exception '{ex}' was filtered with stacktace: {trace}.", context.Exception.Message, context.Exception.StackTrace);
            }
            else
            {
                Log.Information("The exception '{ex}' was filtered an returned status code {code}.", context.Exception.Message, code);
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;

            if (_env.IsProduction())
            {
                context.Result = new JsonResult(new {error = new[] {context.Exception.Message}});
            }
            else
            {
                context.Result = new JsonResult(new
                {
                    error = new[] {context.Exception.Message}, stackTrace = context.Exception.StackTrace
                });
            }
        }
    }
}
