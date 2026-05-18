using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;using
    TaskBoard.Application.Interfaces;
using TaskBoard.Application.Services;
using TaskBoard.Infrastructure.Data;
using TaskBoard.Infrastructure.Repositories;

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
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();

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
// 4. CONFIGURATION AUTHENTIFICATION (JWT)
// ==========================================
var jwtKey = builder.Configuration["Jwt:Key"] ?? "TaCleSecreteSuperLonguePourLeDeveloppement123!"; // À adapter avec ton appsettings.json
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// ==========================================
// 5. CONFIGURATION CONTRÔLEURS ET SWAGGER
// ==========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuration de Swagger pour accepter les Tokens JWT (Le fameux cadenas vert !)
// Configuration de Swagger pour accepter les Tokens JWT (Le cadenas vert !)
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