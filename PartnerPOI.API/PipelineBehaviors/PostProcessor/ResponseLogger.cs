using FastEndpoints;
using FluentValidation.Results;

namespace PartnerPOI.API.PipelineBehaviors.PostProcessor;

public class ResponseLogger: IGlobalPostProcessor
{
    public Task PostProcessAsync(object req, object res, HttpContext ctx, IReadOnlyCollection<ValidationFailure> failures, CancellationToken ct)
    {
        var logger = ctx.Resolve<ILogger<ResponseLogger>>();
        if (failures.Count > 0)
        {
            logger.LogWarning("Validation error count: {@Count}", failures.Count);
        }
        
        logger.LogInformation("Response completed");
        
        return Task.CompletedTask;
    }
}