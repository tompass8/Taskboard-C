using Microsoft.EntityFrameworkCore;
using TaskBoard.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Ajout des contrôleurs
builder.Services.AddControllers();

// Configuration de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injection du DbContext (SQLite pour le développement)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers(); // Mapper les routes des contrôleurs

app.Run();