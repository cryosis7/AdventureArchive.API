using System.Net;
using System.Reflection;
using AdventureArchive.Api.Models;
using AdventureArchive.Api.Services.Doc;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    // .WriteTo.Seq("http://localhost:5341")
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

try
{
    // SelfLog.Enable(Console.Error);
    Log.Information("Starting Adventure Archive API");
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => c.IncludeXmlComments(xmlPath));

    builder.Services.AddControllers();

    builder.Services.Configure<DocApiOptions>(builder.Configuration.GetSection("DocApi"));

    builder.Services.AddHttpClient<IDocHttpClient, DocHttpClient>("DocHttpClient")
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var httpProxy = Environment.GetEnvironmentVariable("HTTP_PROXY");
            if (builder.Environment.IsDevelopment() && !string.IsNullOrEmpty(httpProxy))
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            }

            return new HttpClientHandler();
        });
    builder.Services.AddScoped<IDocService, DocService>();

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