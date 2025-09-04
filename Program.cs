using Microsoft.EntityFrameworkCore;
using PackageTrackingAPI.Data;
using PackageTrackingAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core InMemory DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("PackagesDB"));

// Dependency Injection
builder.Services.AddScoped<IStatusTransitionService, StatusTransitionService>();

// 🚀 Добавляем CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // разрешаем наш фронтенд
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Swagger в Dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// 🚀 Подключаем CORS перед MapControllers()
app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
