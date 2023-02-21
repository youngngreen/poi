//using FastEndpoints;
//using FluentValidation;
//using PartnerPOI.API.Data;
//using PartnerPOI.API.DTOs;

//namespace PartnerPOI.API.Features.PartnerType;

//public class Delete
//{
//    public class Request: BaseRequest
//    {
//        public string? ServiceIdentifiedByPartner { get; set; }
        
//        public int PartnerTypeId { get; set; }

//    }
    
//    public class Response: BaseResponse { }
    
//    public class Validator : Validator<Request>
//    {
//        public Validator()
//        {
//            RuleFor(x => x.ServiceIdentifiedByPartner)
//                .NotNull()
//                .NotEmpty()
//                .WithMessage("ServiceIdentifiedByPartner is required!");

//            RuleFor(x => x.PartnerTypeId)
//                .NotNull()
//                .NotEmpty()
//                .WithMessage("PartnerTypeId is required!");
//        }
//    }
    
//    public class Endpoint : Endpoint<Request>
//    {
//        private readonly PartnerPOIContext _dbContext;
        
//        public Endpoint(PartnerPOIContext dbContext) => _dbContext = dbContext;
        
//        public override void Configure()
//        {
//            Post("/api/partnertype/delete");
//            AllowAnonymous();
//            DontCatchExceptions();
//        }
        
//        public override async Task HandleAsync(Request req, CancellationToken ct)
//        {
//            var partnerType = _dbContext.PartnerType.SingleOrDefault(x => x.Id.Equals(req.PartnerTypeId) 
//                                                                          && x.ServiceIdentifiedByPartner!.Equals(req.ServiceIdentifiedByPartner)
//                                                                          && x.IsDeleted == false);
//            Console.Write("parner type", partnerType);
//            if (partnerType == null)
//            {
//                var m = $"PartnerType not founded by given id = {req.PartnerTypeId}";
//                await SendAsync(new Response
//                {
//                    StatusCode = "000",
//                    Message = m
//                }, cancellation: ct);
                
//                return;
//            }

//            partnerType.IsDeleted = true;
//            partnerType.UpdatedDate = DateTime.UtcNow;
//            partnerType.UpdatedBy = req.UserID;
           
//            _dbContext.PartnerType.Update(partnerType);
//            await _dbContext.SaveChangesAsync(ct);
            
//            var response = new Response
//            {
//                StatusCode = "000",
//                Message = "Successfully",
//            };
            
//            await SendAsync(response, cancellation: ct);
//        }
        
//    }
//}