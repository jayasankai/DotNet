using ShopAppAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ShopContext>(options => options.UseInMemoryDatabase("shop"));

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
