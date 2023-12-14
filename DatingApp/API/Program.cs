using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(opt => {
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultSqliteConnection"));
});

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthentication();
app.MapControllers();

app.Run();
