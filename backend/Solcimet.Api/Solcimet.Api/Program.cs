<<<<<<< Updated upstream
=======
using Microsoft.EntityFrameworkCore;
using Solcimet.Api.Data;
using Solcimet.Api.Services;
using Solcimet.Api.Services.Interfaces;
using Solcimet.Api.Services.LLM;

>>>>>>> Stashed changes
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

<<<<<<< Updated upstream
=======
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOcrService, OcrService>();
builder.Services.AddScoped<IFacturaService, FacturaService>();
builder.Services.AddHttpClient<IGeminiService, GeminiService>();

builder.Services.AddHttpContextAccessor();



builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention()
);

builder.Services.AddScoped<IOcrService, OcrService>();

>>>>>>> Stashed changes
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
