using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TaskBoard.Application.Interfaces;
using TaskBoard.Application.Services;
using TaskBoard.Domain.Interfaces;
using TaskBoard.Infrastructure.Data;
using TaskBoard.Infrastructure.Repositories;
using TaskBoard.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CONFIGURATION DE LA BASE DE DONNÉES
// ==========================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================
// 2. INJECTION DES DÉPENDANCES (Contrats)
// ==========================================
// Auth & Users (Code de Pedro)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Domaines métiers (Ton code)
builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IListRepository, ListRepository>();
builder.Services.AddScoped<IListService, ListService>();

// ==========================================
// 3. CONFIGURATION CORS
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ==========================================
// 4. CONFIGURATION AUTHENTIFICATION (JWT STRICT)
// ==========================================
// On utilise le code de Pedro car il matche parfaitement avec le appsettings.json
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.Zero // Supprime la tolérance de 5min
        };
    });

// ==========================================
// 5. CONFIGURATION SERVICES & SWAGGER
// ==========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR(); // Ajout du SignalR de Pedro

// Configuration de Swagger pour accepter les Tokens JWT (Le cadenas vert !)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskBoard API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Format attendu : 'Bearer {ton_token_jwt}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ==========================================
// 6. PIPELINE HTTP (L'ordre est très important !)
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Ces deux lignes DOIVENT être dans cet ordre exact, juste avant MapControllers
app.UseAuthentication(); // 1. Vérifie QUI tu es (lit le token)
app.UseAuthorization();  // 2. Vérifie ce que tu as le DROIT de faire

app.MapControllers();

app.Run();