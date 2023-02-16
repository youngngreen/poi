using PartnerPOI.API.Data;
using PartnerPOI.API.DTOs;

namespace PartnerPOI.Tests.Features.PartnerType;

public class UpdateTest: IClassFixture<PartnerPOIApplication>
{
    private readonly PartnerPOIApplication _application;
    private readonly HttpClient _client;
    
    public UpdateTest(PartnerPOIApplication application)
    {
        _application = application;
        _client = application.CreateClient();
        _client.DefaultRequestHeaders.Add("userID", "USER_ID");
    }
    
    private static async Task SeederDelegateAsync(PartnerPOIContext dbContext)
    {
        dbContext.PartnerType.RemoveRange(dbContext.PartnerType.ToArray());
        await dbContext.PartnerType.AddAsync(new API.Models.PartnerType
        {
            ServiceIdentifiedByPartner = "Tester",
            Id = 30,
            PartnerTypeNameEN = "prep shop en",
            PartnerTypeNameTH = "prep shop th",
            DisplayOrder = "prep display order",
            IsDeleted = false
        });
        await dbContext.SaveChangesAsync();
    }
    
    [Fact]
    public async Task ShouldUpdateSuccess()
    {
        await _application.Seed(SeederDelegateAsync); 
        
        var (response, result) = await _client
            .PUTAsync<Update.Endpoint, 
                Update.Request, 
                Update.Response>(new()
            {
                ServiceIdentifiedByPartner = "service id",
                PartnerTypeId = 30,
                PartnerTypeNameEN = "top market en",
                PartnerTypeNameTH = "top market th",
                DisplayOrder = "new display order"
            });
        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be("000");
    }
    
    [Fact]
    public async Task ShouldReturnNotFoundGivenInvalidId()
    {
        await _application.Seed(SeederDelegateAsync); 
        
        var (response, result) = await _client
            .PUTAsync<Update.Endpoint, 
                Update.Request, 
                Update.Response>(new()
            {
                ServiceIdentifiedByPartner = "service id",
                PartnerTypeId = 4,
                PartnerTypeNameEN = "top market en",
                PartnerTypeNameTH = "top market th",
                DisplayOrder = "new display order"
            });
        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Message.Should().Contain("PartnerType not founded by given id =");
    }
    
    [Fact]
    public async Task ShouldReturnValueIsMissingResponse()
    {
        await _application.Seed(SeederDelegateAsync); 
        
        var (response, result) = await _client
            .PUTAsync<Update.Endpoint, 
                Update.Request, 
                CustomErrorResponse>(new()
            {
                ServiceIdentifiedByPartner = "service id",
                PartnerTypeId = 30,
                PartnerTypeNameEN = "",
                PartnerTypeNameTH = "top market th",
                DisplayOrder = "new display order"
            });
        
        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be("103");
    }
}