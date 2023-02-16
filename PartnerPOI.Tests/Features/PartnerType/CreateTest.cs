using PartnerPOI.API.DTOs;

namespace PartnerPOI.Tests.Features.PartnerType;

public class CreateTest : IClassFixture<PartnerPOIApplication>
{
    private readonly PartnerPOIApplication _applicationFactory;
    private readonly HttpClient _client;
    
    public CreateTest(PartnerPOIApplication partnerPoiApplication)
    {
        _applicationFactory = partnerPoiApplication;
        _client = partnerPoiApplication.CreateClient();
        _client.DefaultRequestHeaders.Add("userID", "USER_ID");
    }
    
    [Fact]
    public async Task ShouldCreateNewPartnerType()
    {
        var (response, result) = await _client
            .POSTAsync<Create.Endpoint, 
                Create.Request, 
                Create.Response>(new()
                {
                    ServiceIdentifiedByPartner = "service id",
                    PartnerTypeNameEN = "top market en",
                    PartnerTypeNameTH = "top market th",
                    DisplayOrder = "display Order"
                });
    
        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be("000");
    }
    
    [Fact] 
    public async Task ShouldReturnFailedHeaderValidationResponse()
    {
        _client.DefaultRequestHeaders.Remove("userID");
        
        var (response, result) = await _client
            .POSTAsync<Create.Endpoint, 
                Create.Request,
                CustomErrorResponse>(new()
            {
                ServiceIdentifiedByPartner = "service id",
                PartnerTypeNameEN = "top market en",
                PartnerTypeNameTH = "top market th"
            });
    
        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be("100");
    }
    
    [Fact]
    public async Task ShouldReturnMandatoryIsMissingResponse()
    {
        //Create request body without ServiceIdentifiedByPartner
        var (response, result) = await _client  
            .POSTAsync<Create.Endpoint, 
                Create.Request, 
                CustomErrorResponse>(new()
            {
                PartnerTypeNameEN = "",
                PartnerTypeNameTH = "top market th",
                DisplayOrder = "displayOrder"
            });
    
        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be("102");
    }
    
    [Fact]
    public async Task ShouldReturnValueIsMissingResponse()
    {
        //Create request body with PartnerTypeNameEN value = empty string
        var (response, result) = await _client
            .POSTAsync<Create.Endpoint, 
                Create.Request, 
                CustomErrorResponse>(new()
            {
                ServiceIdentifiedByPartner = "service id",
                PartnerTypeNameEN = "",
                PartnerTypeNameTH = "top market th",
                DisplayOrder = "display Order"
            });
    
        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be("103");
    }
}