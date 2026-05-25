using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi; // <-- La clé est ici : PAS de .Models
using Microsoft.OpenApi; // Parfois requis par Swashbuckle, on le laisse si reconnu par d'autres objets, mais on va utiliser ta syntaxe
using TaskBoard.Application.Interfaces;
using TaskBoard.Application.Services;
using TaskBoard.Domain.Interfaces;
using TaskBoard.Infrastructure.Data;
using TaskBoard.Infrastructure.Repositories;
using TaskBoard.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. BASE DE DONNÉES
// ==========================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================
// 2. INJECTIONS DES DÉPENDANCES
// ==========================================
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IListRepository, ListRepository>();
builder.Services.AddScoped<IListService, ListService>();

// ==========================================
// 3. CORS
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// ==========================================
// 4. AUTHENTIFICATION JWT (Stricte)
// ==========================================
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
            ClockSkew = TimeSpan.Zero
        };
    });

// ==========================================
// 5. SERVICES DIVERS
// ==========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();

// ==========================================
// 6. SWAGGER (Ta syntaxe validée .NET 10)
// ==========================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskBoard API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Format attendu : 'Bearer {ton_token_jwt}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    
    // Ton implémentation spécifique qui compile sans erreur
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});

var app = builder.Build();

// ==========================================
// 7. PIPELINE HTTP
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();