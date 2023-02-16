using FastEndpoints;
using FluentValidation;
using PartnerPOI.API.Data;
using PartnerPOI.API.DTOs;

namespace PartnerPOI.API.Features.PartnerType;

public class Create
{
    public class Request: BaseRequest
    {
        public string? ServiceIdentifiedByPartner { get; set; }
        public string PartnerTypeNameEN { get; set; } = null!;
        public string PartnerTypeNameTH { get; set;} = null!;
        public string DisplayOrder { get; set;} = null!;
    }
    
    public class Response: BaseResponse
    {
        public int PartnerTypeID { get; set; }
    }
    
    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ServiceIdentifiedByPartner)
                .NotNull()
                .NotEmpty()
                .WithMessage("ServiceIdentifiedByPartner's is required!");
            
            RuleFor(x => x.PartnerTypeNameEN)
                .NotNull()
                .NotEmpty()
                .WithMessage("PartnerTypeNameEN is required!");

            RuleFor(x => x.PartnerTypeNameTH)
                .NotNull()
                .NotEmpty()
                .WithMessage("PartnerTypeNameTH is required!");
            
            RuleFor(x => x.DisplayOrder)
                .NotNull()
                .NotEmpty()
                .WithMessage("DisplayOrder is required!");
        }
    }

    public class Endpoint : Endpoint<Request>
    {
        private readonly PartnerPOIContext _dbContext;
        
        public Endpoint(PartnerPOIContext dbContext) => _dbContext = dbContext;
        
        public override void Configure()
        {
            Post("/api/partnertype/create");
            AllowAnonymous();
            DontCatchExceptions();
        }
        
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var dateNow = DateTime.UtcNow;
            var model =
                new Models.PartnerType
                {
                    ServiceIdentifiedByPartner = req.ServiceIdentifiedByPartner,
                    PartnerTypeNameEN = req.PartnerTypeNameEN,
                    PartnerTypeNameTH = req.PartnerTypeNameTH,
                    DisplayOrder = req.DisplayOrder,
                    IsDeleted = false,
                    CreatedDate = dateNow,
                    CreatedBy = req.UserID,
                    UpdatedDate = dateNow,
                    UpdatedBy = req.UserID
                };

            await _dbContext.PartnerType.AddAsync(model, ct);
            await _dbContext.SaveChangesAsync(ct);
            
            var response = new Response
            {
                StatusCode = "000",
                Message = "Successfully",
                PartnerTypeID = model.Id
            };
            
            await SendAsync(response, cancellation: ct);
        }
    }
}