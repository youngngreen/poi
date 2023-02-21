using FastEndpoints;
using PartnerPOI.API.Data;
using PartnerPOI.API.DTOs;

namespace PartnerPOI.API.Features.InternalPoint;

public class Delete
{
    public class Validator: Validator<InternalPointDeleteRequest> { }
    public class Endpoint: Endpoint<InternalPointDeleteRequest>
    {
        private readonly PartnerPOIContext _dbContext;
        public Endpoint(PartnerPOIContext dbContext) => _dbContext = dbContext;
        public override void Configure()
        {
            Post("/APFC8013/DeleteInternalPointSetting");
            AllowAnonymous();
            DontCatchExceptions();
        }
        public override async Task HandleAsync(InternalPointDeleteRequest req, CancellationToken ct)
        {
            var internalPoint = _dbContext.TbMInternalPointH.SingleOrDefault(
                    x => x.InternalPointID.Equals(req.InternalPointID)
                        && x.CustomerLevel.Equals(req.CustomerLevel)
                        && x.IsDeleted == false);

            if(internalPoint == null)
            {
                var m = $"Internal point not found by given id = {req.InternalPointID}";
                await SendAsync(new InternalPointDeleteResponse
                {
                    StatusCode = "000",
                    Message = m
                }, cancellation: ct);
                return;
            }

            internalPoint.IsDeleted = true;
            internalPoint.UpdatedDate= DateTime.UtcNow;
            internalPoint.UpdatedBy = req.RequestUserID;

            _dbContext.TbMInternalPointH.Update(internalPoint);
            await _dbContext.SaveChangesAsync(ct);

            var response = new InternalPointDeleteResponse
            {
                StatusCode = "000",
                Message = "Successfully",
            };

            await SendAsync(response, cancellation: ct);

        }
    }
}
