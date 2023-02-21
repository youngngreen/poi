using FastEndpoints;
using FluentValidation.Results;
using PartnerPOI.API.DTOs;
using PartnerPOI.API.Exceptions;

using PartnerPOI.API.Features.PartnerType;

namespace PartnerPOI.API.PipelineBehaviors.PreProcessor;

public class HeaderValidator : IGlobalPreProcessor
{
    public Task PreProcessAsync(object req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
    {
        if (req is BaseRequest r)
        {
            var serviceIdentifiedByPartner = ctx.Request.Headers["ServiceIdentifiedByPartner"].SingleOrDefault();
            if (serviceIdentifiedByPartner == null)
            {
                failures.Add(new("HeaderKey", "Unable to retrieve (serviceIdentifiedByPartner) from header"));
                throw new HeaderValidationException("Header validation failures");
            }

            r.ServiceIdentifiedByPartner = serviceIdentifiedByPartner;
        }

        return Task.CompletedTask;
    }
}
