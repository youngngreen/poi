using FastEndpoints;
using PartnerPOI.API.Data;
using PartnerPOI.API.DTOs;

namespace PartnerPOI.API.Features.InternalPoint;

public class Update
{
    public class Validator: Validator<InternalPointUpdateRequest> { }
    public class Endpoint: Endpoint<InternalPointUpdateRequest>
    {
        private readonly PartnerPOIContext _dbContext;
        public Endpoint(PartnerPOIContext dbContext) => _dbContext = dbContext;
        public override void Configure()
        {
            Put("/APFC8012/EditInternalPointSetting");
            AllowAnonymous();
            DontCatchExceptions();
        }
        public override async Task HandleAsync(InternalPointUpdateRequest req, CancellationToken ct)
        {
            var internalPointH = _dbContext.TbMInternalPointH.SingleOrDefault(
                x => x.InternalPointID.Equals(req.InternalPointID)
                && x.CustomerLevel.Equals(req.CustomerLevel)
             );
            if(internalPointH == null)
            {
                var m = $"Internal Point not founded by given id = {req.InternalPointID}";
                await SendAsync(new InternalPointUpdateResponse
                {
                    StatusCode = "000",
                    Message = m
                }, cancellation: ct);
                return;
            }

            // (3). Update Internal Point Setting
            // 3.1 Update Internal Point Setting Header
            internalPointH.UpdatedDate = DateTime.UtcNow; // GETUTCDATE() -- waiting for COMMON
            internalPointH.UpdatedBy = req.RequestUserID;
            _dbContext.TbMInternalPointH.Update(internalPointH);
            await _dbContext.SaveChangesAsync(ct);

            // 3.2 Delete Internal Point Setting Detail
            // query to get InternalPointSettingID from Header Table (internalPointH)
            var internalPointD = _dbContext.TbMInternalPointD.Where(
                x => x.InternalPointSettingID.Equals(internalPointH.InternalPointSettingID)).ToList();

            Console.WriteLine("ID0 " + internalPointD[0]);
            Console.WriteLine("ID1 " + internalPointD[1]);

            for(int i = 0; i < internalPointD.Count; i++)
            {
                _dbContext.TbMInternalPointD.Remove(internalPointD[i]);
                await _dbContext.SaveChangesAsync(ct);
            }


            //_dbContext.TbMInternalPointD.Remove(internalPointD);
            //await _dbContext.SaveChangesAsync(ct);

            // 3.3 Insert Internal Point Setting Detail

            // (4). Prepare API Response
            var response = new InternalPointUpdateResponse
            {
                StatusCode = "000",
                Message = "Successfully"
            };
            await SendAsync(response, cancellation: ct);
        }
    }
}
