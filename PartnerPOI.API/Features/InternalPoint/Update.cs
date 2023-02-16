using FastEndpoints;
using PartnerPOI.API.Data;
using PartnerPOI.API.DTOs;

namespace PartnerPOI.API.Features.InternalPoint;

public class Update
{
    public class Event
    {
        public int unitPerPoint { get; set; }
        public string expireDatePattern { get; set; } = null!;
        public int multiplier { get; set; }
        public string effectiveFrom { get; set; } = null!;
        public string effectiveTo { get; set; } = null!;
    }
    public class Request: BaseRequest
    {
        public string requestUserID { get; set; }
        public string serviceIdentifiedByPartner { get; set; }
        public string internalPointID { get; set; }
        public string customerLevel { get; set; }
        public Event[] eventList { get; set; }

    }
    public class Response: BaseResponse { }
    public class Validator: Validator<Request> { }
    public class Endpoint: Endpoint<Request>
    {
        private readonly PartnerPOIContext _dbContext;
        public Endpoint(PartnerPOIContext dbContext) => _dbContext = dbContext;
        public override void Configure()
        {
            Put("/APFC8012/EditInternalPointSetting");
            AllowAnonymous();
            DontCatchExceptions();
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var internalPoint = _dbContext.TbMInternalPointH.SingleOrDefault(
                x => x.serviceIdentifiedByPartner.Equals(req.serviceIdentifiedByPartner)
                && x.internalPointID.Equals(req.internalPointID)
                && x.customerLevel.Equals(req.customerLevel)
             );
            if(internalPoint == null)
            {
                var m = $"Internal Point not founded by given id = {req.internalPointID}";
                await SendAsync(new Response
                {
                    StatusCode = "000",
                    Message = m
                }, cancellation: ct);
                return;
            }

            internalPoint.updatedDate = DateTime.UtcNow;
            internalPoint.updatedBy = req.UserID;

            _dbContext.TbMInternalPointH.Update(internalPoint);
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
