using FloraFauna_GO_Dto;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_Go_Repository;
using FloraFauna_GO_Shared;
using FloraFaunaGO_API.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
});

var init = new AppBootstrap(builder.Configuration);

init.ConfigureServices(builder.Services);

var app = builder.Build();

init.Configure(app, app.Environment);

var context = app.Services.GetService<FloraFaunaGoDB>();
context!.Database.EnsureCreated();

Console.WriteLine("============ Database created ============");
app.Run();

Console.WriteLine($"API running on {app.Urls}");