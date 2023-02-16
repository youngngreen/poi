using PartnerPOI.API.Data;

namespace PartnerPOI.Tests.Features.PartnerType;

public class DeleteTest: IClassFixture<PartnerPOIApplication>
{
    private readonly PartnerPOIApplication _application;
    private readonly HttpClient _client;
    
    public DeleteTest(PartnerPOIApplication application)
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
            DisplayOrder = "prep display order"
        });
        await dbContext.SaveChangesAsync();
    }
    
    [Fact]
    public async Task ShouldDeleteSuccess()
    {
        await _application.Seed(SeederDelegateAsync);

        var (response, _) = await _client
            .POSTAsync<Delete.Endpoint, 
                Delete.Request, 
                Delete.Response>(new()
            {
                ServiceIdentifiedByPartner = "Tester",
                PartnerTypeId = 30,
            });
        
        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task ShouldReturnNotFoundGivenInvalidId()
    {
        await _application.Seed(SeederDelegateAsync);

        var (response, result) = await _client
            .POSTAsync<Delete.Endpoint, 
                Delete.Request, 
                Delete.Response>(new()
            {
                ServiceIdentifiedByPartner = "Tester",
                PartnerTypeId = 4,
            });
        
        // Assert
        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Message.Should().Contain("PartnerType not founded by given id =");
    }
}