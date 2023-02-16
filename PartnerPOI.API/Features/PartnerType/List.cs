using FastEndpoints;
using FluentValidation;
using PartnerPOI.API.Data;
using PartnerPOI.API.DTOs;

namespace PartnerPOI.API.Features.PartnerType;

public class List
{
    public class Response<T>: BaseResponse
    {
        public T? Result { get; set; }
    }
    
    public class Endpoint : EndpointWithoutRequest<Response<List<Models.PartnerType>>>
    {
        private readonly PartnerPOIContext _dbContext;
        
        public Endpoint(PartnerPOIContext dbContext) => _dbContext = dbContext;
        
        public override void Configure()
        {
            Get("/api/partnertypes");
            AllowAnonymous();
            DontCatchExceptions();
        }
        
        public override async Task HandleAsync(CancellationToken ct)
        {
            var results = _dbContext.PartnerType.ToList();
            var response = new Response<List<Models.PartnerType>>
            {
                StatusCode = "000",
                Message = "Successfully",
                Result = results
            };
            await SendAsync(response, cancellation: ct);
        }
    }
}