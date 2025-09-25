using GrpcService1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

// Configure Kestrel to listen on all interfaces for containerized environments
builder.WebHost.ConfigureKestrel(options =>
{
    // Listen on all interfaces, port determined by ASPNETCORE_URLS environment variable
    // Default fallback to port 5117 for development
    var port = Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORTS") != null ?
               int.Parse(Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORTS")!) : 5117;
    options.ListenAnyIP(port);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
