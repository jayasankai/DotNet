using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(opt => {
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultSqliteConnection"));
});

// CORS support
builder.Services.AddCors();

var app = builder.Build();
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
app.UseHttpsRedirection();
app.UseAuthentication();
app.MapControllers();

app.Run();
