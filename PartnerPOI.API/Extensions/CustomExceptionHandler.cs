using System.Diagnostics;
using System.Net;
using System.Text.Json;
using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Diagnostics;
using PartnerPOI.API.DTOs;
using PartnerPOI.API.Exceptions;
using PartnerPOI.API.Features.PartnerType;

namespace PartnerPOI.API.Extensions;

internal class ExceptionHandler { }

public static class CustomExceptionHandler
{
    /// <summary>
    /// registers the default global exception handler which will log the exceptions on the server and return a user-friendly json response to the client when unhandled exceptions occur.
    /// TIP: when using this exception handler, you may want to turn off the asp.net core exception middleware logging to avoid duplication like so:
    /// <code>
    /// "Logging": { "LogLevel": { "Default": "Warning", "Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware": "None" } }
    /// </code>
    /// </summary>
    /// <param name="logger">an optional logger instance</param>
    public static void UseCustomExceptionHandler(this IApplicationBuilder app, ILogger? logger = null)
    {
        app.UseExceptionHandler(errApp =>
        {
            errApp.Run(async ctx =>
            {
                var exHandlerFeature = ctx.Features.Get<IExceptionHandlerFeature>();
                if (exHandlerFeature is not null)
                {
                    var httpStatusCode = (int)HttpStatusCode.InternalServerError;
                    var errorMessage = exHandlerFeature.Error.Message;
                    var responseStatusCode = "900";
                    var responseMessage = "Unhandled exception error";
                    
                    //Validation Failure
                    if (exHandlerFeature.Error is ValidationFailureException vd && vd.Failures!.Any())
                    {
                        var failure = vd.Failures!.First();
                        httpStatusCode = (int)HttpStatusCode.BadRequest;
                        switch (failure.ErrorCode)
                        {
                            case "NotNullValidator":
                                responseStatusCode = "102";
                                responseMessage = $"Mandatory key ({failure.PropertyName}) is missing";
                                break;
                            case "NotEmptyValidator" :
                                responseStatusCode = "103";
                                responseMessage = $"Value ({failure.PropertyName}) is missing";
                                break;
                            default:
                                responseStatusCode = "104";
                                responseMessage = "Invalid data";
                                break;
                        };
                    } 
                    else if (exHandlerFeature.Error is HeaderValidationException)
                    {
                        httpStatusCode = (int)HttpStatusCode.BadRequest;
                        responseStatusCode = "100";
                        responseMessage = "Mandatory HTTP header is missing";
                    }
                    else if (exHandlerFeature.Error is JsonException)
                    {
                        httpStatusCode = (int)HttpStatusCode.BadRequest;
                        responseStatusCode = "101";
                        responseMessage = "Invalid JSON format";
                    }
                    
                    logger ??= ctx.RequestServices.GetRequiredService<ILogger<ExceptionHandler>>();
                    logger.LogError(errorMessage);
                    
                    ctx.Response.StatusCode = httpStatusCode;
                    await ctx.Response.WriteAsJsonAsync(new CustomErrorResponse(responseStatusCode, responseMessage));
                }
            });
        });
    }
}