using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using TaskBoard.Api.Middleware;
using TaskBoard.Api.Hubs;
using TaskBoard.Api.Services;
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
// 2. INJECTION DES DÉPENDANCES (Nos contrats)
// ==========================================
// C'est ici qu'on dit à l'API quel code utiliser quand un contrôleur demande une interface

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IListRepository, ListRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();

// Services
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IListService, ListService>();
builder.Services.AddScoped<ICardService, CardService>();

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
// 4. GESTION DU CRYPTAGE DU MDP (BCrypt)
// ==========================================
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ==========================================
// 5. CONFIGURATION AUTHENTIFICATION (JWT)
// ==========================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes
            (builder.Configuration["Jwt:Key"]!)),
        ClockSkew = TimeSpan.Zero, // delete 5min time tolerance
        NameClaimType = "username"
    };
    
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var cookieToken = context.Request.Cookies["jwt"];
            var queryToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            
            if (!string.IsNullOrEmpty(queryToken) && path.StartsWithSegments("/hubs"))
                context.Token = queryToken;
            else if (!string.IsNullOrEmpty(cookieToken))
                context.Token = cookieToken;
            
            return Task.CompletedTask;
        }
    };
});

// ==========================================
// 6. GESTION DE SIGNALR
// ==========================================
builder.Services.AddSignalR();

// Notification Board Hub
builder.Services.AddScoped<IBoardNotificationService, BoardNotificationService>();

// ==========================================
// 7. CONFIGURATION CONTRÔLEURS ET SWAGGER
// ==========================================
builder.Services.AddControllers();

// Configuration de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
// Configuration de Swagger pour accepter les Tokens JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskBoard API", Version = "v1" });
    
    // Nouvelle façon de déclarer la sécurité
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Format attendu : 'Bearer {ton_token_jwt}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    
    // Nouvelle façon d'exiger le token (avec le mot-clé 'document')
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        
    });
});

var app = builder.Build();

// ==========================================
// 8. PIPELINE HTTP (L'ordre est très important !)
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ErrorHandlingMiddleware>(); // Must be FIRST
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication(); // Must be BEFORE Authorization | Vérifie QUI tu es (lit le Token)
app.UseAuthorization(); // Must be AFTER Authentification | Vérifie ce que tu as le DROIT de faire
app.MapControllers();
app.MapHub<BoardHub>("/hubs/board");

app.Run();