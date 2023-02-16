using FastEndpoints;
using Microsoft.AspNetCore.Routing.Constraints;
using PartnerPOI.API.Data;
using PartnerPOI.API.DTOs;

namespace PartnerPOI.API.Features.InternalPoint;

public class Get
{
    public class Request: BaseRequest
    {
        //public int internalPointSettingID { get; set; }

        public string? requestUserID { get; set; }
        public string? serviceIdentifiedByPartner { get; set; }
        public string? internalPointID { get; set; }
        public string? customerLevel { get; set; }
        public bool isDeleted { get; set; }
        public int startRecord { get; set; }
        public int noOfRecord { get; set; }
    }
    public class Response<T> : BaseResponse
    {
        public int retrieveRecord { get; set; }
        public int totalRecord { get; set; }
        public T? Result { get; set; }
    }
    public class Endpoint: Endpoint<Request>
    {
        private readonly PartnerPOIContext _dbContext;
        public Endpoint(PartnerPOIContext dbContext) => _dbContext = dbContext;
        public override void Configure()
        {
            Post("/APFC8014/GetinternalPointSetting");
            AllowAnonymous();
            DontCatchExceptions();
        }
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var totalRows = _dbContext.TbMInternalPointH.Where(
                x => x.serviceIdentifiedByPartner.Equals(req.serviceIdentifiedByPartner)
                    && x.internalPointID.Equals(req.internalPointID)
                    && x.customerLevel.Equals(req.customerLevel)
                    && x.isDeleted.Equals(req.isDeleted)
                ).ToList();

            var result = _dbContext.TbMInternalPointH.Where(
                x => x.serviceIdentifiedByPartner.Equals(req.serviceIdentifiedByPartner)
                    && x.internalPointID.Equals(req.internalPointID)
                    && x.customerLevel.Equals(req.customerLevel)
                    && x.isDeleted.Equals(req.isDeleted)
                ).Skip(req.startRecord - 1).Take(req.noOfRecord).ToList();

            Console.WriteLine("rs length" + totalRows.Count.ToString());

            var response = new Response<List<Models.InternalPoint>>
            {
                retrieveRecord = req.noOfRecord,
                totalRecord = totalRows.Count,
                StatusCode = "000",
                Message = "Successfully",
                Result = result
            };
            await SendAsync(response, cancellation: ct);
        }
    }

}
