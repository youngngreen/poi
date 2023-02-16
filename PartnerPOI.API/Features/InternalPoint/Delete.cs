using FastEndpoints;
using PartnerPOI.API.Data;
using PartnerPOI.API.DTOs;

namespace PartnerPOI.API.Features.InternalPoint;

public class Delete
{
    public class Request: BaseRequest
    {
        public string requestUserID { get; set; }
        public string serviceIdentifiedByPartner { get; set; }
        public string partnerPoinID { get; set; }
        public string customerJourneyID { get; set; }
    }
    public class Response : BaseResponse { }
    public class Validator: Validator<Request> { }
    public class Endpoint: Endpoint<Request>
    {
        private readonly PartnerPOIContext _dbContext;
        public Endpoint(PartnerPOIContext dbContext) => _dbContext = dbContext;
        public override void Configure()
        {
            Post("/APFC8013/DeleteInternalPointSetting");
            AllowAnonymous();
            DontCatchExceptions();
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var internalPoint = _dbContext.TbMInternalPointH.SingleOrDefault(
                    x => x.serviceIdentifiedByPartner.Equals(req.serviceIdentifiedByPartner)
                        && x.isDeleted == false);

            if(internalPoint == null)
            {
                var m = $"Internal point not found by given id = {req.partnerPoinID}";
                await SendAsync(new Response
                {
                    StatusCode = "000",
                    Message = m
                }, cancellation: ct);
                return;
            }

            internalPoint.isDeleted = true;
            internalPoint.updatedDate= DateTime.UtcNow;
            internalPoint.updatedBy = req.UserID;

            _dbContext.TbMInternalPointH.Update(internalPoint);
            await _dbContext.SaveChangesAsync(ct);

            var response = new Response
            {
                StatusCode = "000",
                Message = "Successfully",
            };

            await SendAsync(response, cancellation: ct);

        }
    }
}
