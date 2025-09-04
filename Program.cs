using Microsoft.EntityFrameworkCore;
using PackageTrackingAPI.Data;
using PackageTrackingAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        // Чтобы в JSON enum статусов шёл строкой: "Created", "Sent", ...
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core InMemory DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("PackagesDB"));

// DI: правила переходов статусов
builder.Services.AddScoped<IStatusTransitionService, StatusTransitionService>();

var app = builder.Build();

// Swagger в Dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // если ругается на https — не критично, работаем по http
app.UseAuthorization();
app.MapControllers();
app.Run();
