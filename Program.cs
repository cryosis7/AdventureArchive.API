using System.Net;
using AdventureArchive.Api.Services;

var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.IncludeXmlComments(xmlPath));

builder.Services.AddControllers();

builder.Services.AddHttpClient<IDocService, DocService>("DocService")
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();