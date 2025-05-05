using System.Reflection;
using AdventureArchive.Api.Api.Extensions;
using AdventureArchive.Api.Infrastructure.ExternalServices.DocApi;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

try
{
    Log.Information("Starting Adventure Archive API");
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => c.IncludeXmlComments(xmlPath));

    builder.Services.AddControllers();

    builder.Services.Configure<DocApiOptions>(builder.Configuration.GetSection("DocApi"));
    
    builder.Services.RegisterDependencies(isDevelopment: builder.Environment.IsDevelopment());

    var app = builder.Build();
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

namespace AdventureArchive.Api.Api
{
}