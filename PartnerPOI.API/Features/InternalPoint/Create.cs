using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using PartnerPOI.API.Data;
using PartnerPOI.API.DTOs;

namespace PartnerPOI.API.Features.InternalPoint;

public class Create
{
    public class Event
    {
        public int unitPerPoint { get; set; }
        public string expireDatePattern { get; set; } = null!;
        public int multiplier { get; set; }
        public string effectiveFrom { get; set; } = null!;
        public string effectiveTo { get; set; } = null!; 
    }
    public class Request : BaseRequest
    {
        ///
        public string? requestUserID { get; set; }
        public string? serviceIdentifiedByPartner {get; set; }
        public string? internalPointID {get; set; }
        public string? internalPointDesc { get; set; }
        public string? customerLevel { get; set; }
        public Event[]? eventList { get; set; }


    }
    public class Response: BaseResponse
    {

    }
    public class Validator: Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.requestUserID)
                .NotNull()
                .NotEmpty()
                .WithMessage("requestUserID's is required!");
        }
    }
    public class Endpoint: Endpoint<Request>
    {
        private readonly PartnerPOIContext _dbContext;
        public Endpoint(PartnerPOIContext dbContext) => _dbContext = dbContext;
        public override void Configure()
        {
            Post("/APFC8011/CreateInternalPointSetting");
            AllowAnonymous();
            DontCatchExceptions();
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var dateNow = DateTime.UtcNow;
            var model = new Models.InternalPoint
            {
                //internalPointSettingID = "xx",
                serviceIdentifiedByPartner=req.serviceIdentifiedByPartner,
                internalPointID=req.internalPointID,
                internalPointDesc = req.internalPointDesc,
                customerLevel = req.customerLevel,
                isDeleted = false,
                createdDate = dateNow,
                createdBy = "user ID" + req.requestUserID,
                updatedDate = dateNow,
                updatedBy = "user ID" + req.requestUserID,
            };
            await _dbContext.TbMInternalPointH.AddAsync(model, ct);
            await _dbContext.SaveChangesAsync(ct);
            var response = new Response
            {
                StatusCode = "000",
                Message = "Successfully"
            };
            await SendAsync(response, cancellation: ct);
        }
    }
}
