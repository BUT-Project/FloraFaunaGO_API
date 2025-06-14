using FloraFauna_GO_Entities;
using FloraFaunaGO_API.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
});

var init = new AppBootstrap(builder.Configuration);

init.ConfigureServices(builder.Services);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FloraFaunaGoDB>();
    dbContext.Database.EnsureCreated();
    Console.WriteLine("============ Database created with Identity tables ============");
}

init.Configure(app, app.Environment);

Console.WriteLine("============ Database created ============");
app.Run();

Console.WriteLine($"API running on {app.Urls}");