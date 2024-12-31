using FloraFauna_GO_Entities;
using FloraFauna_Go_Repository;
using FloraFauna_GO_Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FloraFaunaGoDB>();
builder.Services.AddScoped<IUnitOfWork<EspeceEntities, CaptureEntities, UtilisateurEntities>, UnitOfWork>();
builder.Services.AddScoped<IUserRepository<UtilisateurEntities>, UserRepository>();
builder.Services.AddScoped<ICaptureRepository<CaptureEntities>, CaptureRepository>();
builder.Services.AddScoped<IEspeceRepository<EspeceEntities>, EspeceRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
