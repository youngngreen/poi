using FastEndpoints;
using FluentValidation;
using PartnerPOI.API.Data;
using PartnerPOI.API.DTOs;
using static PartnerPOI.API.DTOs.InternalPointCreateRequest;

namespace PartnerPOI.API.Features.InternalPoint;

public class Create
{
    public class Validator: Validator<InternalPointCreateRequest>
    {
        public Validator()
        {
            RuleFor(x => x.RequestUserID)
                .NotNull()
                .NotEmpty()
                .WithMessage("requestUserID's is required!");
        }
    }
    public class Endpoint: Endpoint<InternalPointCreateRequest>
    {
        private readonly PartnerPOIContext _dbContext;
        public Endpoint(PartnerPOIContext dbContext) => _dbContext = dbContext;
        public override void Configure()
        {
            Post("/APFC8011/CreateInternalPointSetting");
            AllowAnonymous();
            DontCatchExceptions();
        }
        public override async Task HandleAsync(InternalPointCreateRequest req, CancellationToken ct)
        {
            var dateNow = DateTime.UtcNow;
            // (3). Create Internal Point Setting
            // 3.1 Insert Internal Point Setting Header
            var model = new Models.TbMInternalPointH
            {
                InternalPointID = req.InternalPointID,
                InternalPointDesc = req.InternalPointDesc,
                CustomerLevel = req.CustomerLevel,
                IsDeleted = false,
                CreatedDate = dateNow,
                CreatedBy = "user ID" + req.RequestUserID,
                UpdatedDate = dateNow,
                UpdatedBy = "user ID" + req.RequestUserID
            };
            await _dbContext.TbMInternalPointH.AddAsync(model, ct);
            var data1 = await _dbContext.SaveChangesAsync(ct);

            // 3.2 Insert Internal Point Setting Detail
            int i = 1; // for counting the index
            foreach (EventList Event in req.EventLists)
            {
                Random rnd = new Random();
                var detailModel = new Models.TbMInternalPointD
                { 
                    InternalPointSettingID = model.InternalPointSettingID,
                    SeqNo = i,
                    UnitPerPoint = (int)Event.UnitPerPoint,
                    Multiplier = (int)Event.Multiplier,
                    EffectiveFrom = DateTime.Parse(Event.EffectiveFrom),
                    EffectiveTo = DateTime.Parse(Event.EffectiveTo),
                    ExpireDatePattern = Event.ExpireDatePattern,
                    CreatedDate = dateNow,
                    CreatedBy = req.RequestUserID,
                    UpdatedDate = dateNow,
                    UpdatedBy = req.RequestUserID
                };
                await _dbContext.TbMInternalPointD.AddAsync(detailModel, ct);
                await _dbContext.SaveChangesAsync(ct);
                i++;
            }

            // (4). Prepare Normal API Response
            var response = new InternalPointCreateResponse
            {
                StatusCode = "000",
                Message = "Successfully"
            };
            await SendAsync(response, cancellation: ct);
        }
    }
}
