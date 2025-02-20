using System.Diagnostics.CodeAnalysis;
using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using Tusk.Application.Exceptions;

namespace Tusk.Api.Filters;

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
        var (code, message) = context.Exception switch
        {
            InvalidOperationException or ValidationException _ => (HttpStatusCode.BadRequest, "Operation failed"),
            NotFoundException _ => (HttpStatusCode.NotFound, "Item not found"),
            _ => (HttpStatusCode.InternalServerError, "Unknown internal error"),
        };

        if (code == HttpStatusCode.InternalServerError)
        {
            Log.Error("The unhandled internal exception '{Ex}' was filtered with stacktrace: {Trace}",
                context.Exception.Message, context.Exception.StackTrace);
        }
        else
        {
            Log.Information("The exception '{Ex}' was filtered an returned status code {Code}",
                context.Exception.Message, code);
        }

        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.StatusCode = (int)code;
        var returnMessage = context.Exception switch
        {
            ValidationException ex => new JsonResult(
                ex.Errors.GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    )),
            _ => new JsonResult(new {error = new[] {context.Exception.Message}})
        };

        context.Result = _env!.IsProduction()
            ? returnMessage
            : new JsonResult(new {error = returnMessage, stackTrace = context.Exception.StackTrace});
    }
}
