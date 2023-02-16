using PartnerPOI.API.Data;

namespace PartnerPOI.Tests.Features.PartnerType;

public class GetByIdTest: IClassFixture<PartnerPOIApplication>
{
    private readonly PartnerPOIApplication _application;
    private readonly HttpClient _client;
    
    public GetByIdTest(PartnerPOIApplication application)
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
        
        await dbContext.PartnerType.AddAsync(new API.Models.PartnerType
        {
            ServiceIdentifiedByPartner = "Tester",
            Id = 31,
            PartnerTypeNameEN = "prep shop en 2",
            PartnerTypeNameTH = "prep shop th 2",
            DisplayOrder = "prep display order 2"
        });
        
        await dbContext.PartnerType.AddAsync(new API.Models.PartnerType
        {
            ServiceIdentifiedByPartner = "Tester",
            Id = 32,
            PartnerTypeNameEN = "prep shop en 3",
            PartnerTypeNameTH = "prep shop th 3",
            DisplayOrder = "prep display order 3"
        });
        
        await dbContext.SaveChangesAsync();
    }
    
    [Fact]
    public async Task ShouldReturnPartnerTypeGivenValidId()
    {
        await _application.Seed(SeederDelegateAsync);

        var (response, result) = await _client
            .GETAsync<GetById.Endpoint, 
                GetById.Request, 
                GetById.Response<API.Models.PartnerType>>(new()
            {
                PartnerTypeId = 30,
            });
        
        // Assert
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result!.Result.Should().NotBeNull();
    }
}