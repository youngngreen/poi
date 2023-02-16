using FastEndpoints;
using FluentValidation.Results;
namespace PartnerPOI.API.PipelineBehaviors.PreProcessor;

public class RequestLogger: IGlobalPreProcessor
{
    public Task PreProcessAsync(object req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
    {
        var logger = ctx.RequestServices.GetRequiredService<ILogger<RequestLogger>>();

        logger.LogInformation("Request:{FullName}, path: {RequestPath}", req?.GetType().FullName, ctx.Request.Path);

        return Task.CompletedTask;
    }
}