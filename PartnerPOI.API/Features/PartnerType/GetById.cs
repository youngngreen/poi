using FastEndpoints;
using FluentValidation;
using PartnerPOI.API.Data;
using PartnerPOI.API.DTOs;

namespace PartnerPOI.API.Features.PartnerType;

public class GetById
{
    public class Request: BaseRequest
    {
        public int PartnerTypeId { get; set; }
    }
    
    public class Response<T>: BaseResponse
    {
        public T? Result { get; set; }
    }
    
    public class Endpoint : Endpoint<Request>
    {
        private readonly PartnerPOIContext _dbContext;
        
        public Endpoint(PartnerPOIContext dbContext) => _dbContext = dbContext;
        
        public override void Configure()
        {
            Get("/api/partnertypes/{PartnerTypeId}");
            AllowAnonymous();
            DontCatchExceptions();
        }
        
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var result = _dbContext.PartnerType.SingleOrDefault(x => x.Id.Equals(req.PartnerTypeId));
            var response = new Response<Models.PartnerType>
            {
                StatusCode = "000",
                Message = "Successfully",
                Result = result
            };
            await SendAsync(response, cancellation: ct);
        }
    }
}