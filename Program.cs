using System.Net;
using AdventureArchive.Api.Services;

var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.IncludeXmlComments(xmlPath));

builder.Services.AddControllers();

var proxyAddress = builder.Configuration.GetSection("ProxySettings")["Address"];
builder.Services.AddHttpClient<DocService>("DocService")
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
// builder.Services.AddHttpClient<DocService>("DocService")
//     .ConfigurePrimaryHttpMessageHandler(() =>
//     {
//         var proxy = proxyAddress != null
//             ? new WebProxy
//             {
//                 Address = new Uri(proxyAddress),
//                 BypassProxyOnLocal = false
//             }
//             : null;
//
//         if (builder.Environment.IsDevelopment())
//         {
//             return new HttpClientHandler
//             {
//                 Proxy = proxy,
//                 UseProxy = true,
//                 ServerCertificateCustomValidationCallback =
//                     HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
//             };
//         }
//
//         return new HttpClientHandler
//         {
//             Proxy = proxy,
//             UseProxy = proxy != null
//         };
//     });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// app.UseCors(policy =>
//     policy.SetIsOriginAllowed((origin) => origin)
//         .AllowAnyMethod()
//         .AllowAnyHeader());

app.Run();