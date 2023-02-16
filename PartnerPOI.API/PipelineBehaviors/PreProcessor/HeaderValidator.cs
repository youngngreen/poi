using FastEndpoints;
using FluentValidation.Results;
using PartnerPOI.API.DTOs;
using PartnerPOI.API.Exceptions;

using PartnerPOI.API.Features.PartnerType;

namespace PartnerPOI.API.PipelineBehaviors.PreProcessor;

public class HeaderValidator: IGlobalPreProcessor
{
    public Task PreProcessAsync(object req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
    {
        if (req is BaseRequest r)
        {
            var userId = ctx.Request.Headers["userID"].SingleOrDefault();
            if (userId == null)
            {
                failures.Add(new("HeaderKey", "Unable to retrieve (userID) from header"));
                throw new HeaderValidationException("Header validation failures");
            }
            
            r.UserID = userId;
        }
        
        return Task.CompletedTask;
    }
}