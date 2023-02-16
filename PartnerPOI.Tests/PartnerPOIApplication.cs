using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using PartnerPOI.API.Data;

namespace PartnerPOI.Tests;

public class PartnerPOIApplication: WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging => logging.ClearProviders());
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped(sp => new DbContextOptionsBuilder<PartnerPOIContext>()
                .UseInMemoryDatabase("PartnerPOIContext")
                .UseApplicationServiceProvider(sp)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options);
        });
    }

    public Task Seed(Func<PartnerPOIContext, Task> seeder)
    {
        using var seedScope = Services.CreateScope();
        var db = seedScope.ServiceProvider.GetRequiredService<PartnerPOIContext>();
        return seeder(db);
    }
}