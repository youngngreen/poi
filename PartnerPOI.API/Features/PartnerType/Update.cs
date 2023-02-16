using FastEndpoints;
using FluentValidation;
using PartnerPOI.API.Data;
using PartnerPOI.API.DTOs;

namespace PartnerPOI.API.Features.PartnerType;

public class Update
{
    public class Request: BaseRequest
    {
        public string? ServiceIdentifiedByPartner { get; set; }
        public int PartnerTypeId { get; set; }
        public string PartnerTypeNameEN { get; set; } = null!;
        public string PartnerTypeNameTH { get; set;} = null!;
        
        public string DisplayOrder { get; set;} = null!;
    }
    
    public class Response: BaseResponse { }
    
    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ServiceIdentifiedByPartner)
                .NotNull()
                .NotEmpty()
                .WithMessage("ServiceIdentifiedByPartner is empty!");
            
            RuleFor(x => x.PartnerTypeId)
                .NotNull()
                .NotEmpty()
                .WithMessage("PartnerTypeId is empty!");
            
            RuleFor(x => x.PartnerTypeNameEN)
                .NotNull()
                .NotEmpty()
                .WithMessage("PartnerTypeNameEN is empty!");

            RuleFor(x => x.PartnerTypeNameTH)
                .NotNull()
                .NotEmpty()
                .WithMessage("PartnerTypeNameTH is empty!");
            
            RuleFor(x => x.DisplayOrder)
                .NotNull()
                .NotEmpty()
                //.Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("DisplayOrder is empty!");
        }
    }
    
    public class Endpoint : Endpoint<Request>
    {
        private readonly PartnerPOIContext _dbContext;
        
        public Endpoint(PartnerPOIContext dbContext) => _dbContext = dbContext;
        
        public override void Configure()
        {
            Put("/api/partnertype/update");
            AllowAnonymous();
            DontCatchExceptions();
        }
        
        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var partnerType = _dbContext.PartnerType.SingleOrDefault(x => x.Id.Equals(req.PartnerTypeId) 
                                                                          && x.ServiceIdentifiedByPartner!.Equals(req.ServiceIdentifiedByPartner)
                                                                          && x.IsDeleted == false);
            if (partnerType == null)
            {
                var m = $"PartnerType not founded by given id = {req.PartnerTypeId}";
                await SendAsync(new Response
                {
                    StatusCode = "000",
                    Message = m
                }, cancellation: ct);
                
                return;
            }
            
            partnerType.PartnerTypeNameEN = req.PartnerTypeNameEN;
            partnerType.PartnerTypeNameTH = req.PartnerTypeNameTH;
            partnerType.DisplayOrder = req.DisplayOrder;
            partnerType.UpdatedDate = DateTime.UtcNow;
            partnerType.UpdatedBy = req.UserID;
           
            _dbContext.PartnerType.Update(partnerType);
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