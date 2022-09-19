using ApiLogging.Data;
using ApiLogging.EntitiesCQ.Interface;
using ApiLogging.EntitiesCQ.Mongo.Interface;
using ApiLogging.EntitiesCQ.Mongo.Service;
using ApiLogging.EntitiesCQ.Service;
using ApiLogging.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddScoped<IMongoRepository, MongoRepository>();
builder.Services.AddScoped<ILoggingService, LoggingService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<TestDbContext>(options =>
                options.UseNpgsql(configuration["ConnectionStrings:Connect"]));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<LoggerMiddleware>();
app.MapControllers();

app.Run();
