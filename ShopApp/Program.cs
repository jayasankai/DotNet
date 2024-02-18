using ShopAppAPI.Data;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApiVersioning(options => {
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
}).AddMvc();

builder.Services.AddDbContext<ShopContext>(options => options.UseInMemoryDatabase("shop"));

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
