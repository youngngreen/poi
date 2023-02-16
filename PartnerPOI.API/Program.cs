using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PartnerPOI.API.Data;
using PartnerPOI.API.DTOs;
using PartnerPOI.API.Extensions;
using PartnerPOI.API.PipelineBehaviors.PostProcessor;
using PartnerPOI.API.PipelineBehaviors.PreProcessor;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();
builder.Services.AddMemoryCache();
builder.Services.AddDaprClient();
//Data
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PartnerPOIContext>(options => options
    .UseMySQL(connectionString!)
    //.UseInMemoryDatabase("PartnerPOIContextMemoryDB")
    //.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

var app = builder.Build();
//app.UseCustomExceptionHandler();
app.UseAuthorization();
app.UseFastEndpoints(c =>
{
    c.Endpoints.Configurator = ep =>
    {
        ep.PreProcessors(Order.Before, new RequestLogger());
        ep.PreProcessors(Order.Before, new HeaderValidator());
        ep.PostProcessors(Order.After, new ResponseLogger());
    };
});

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }