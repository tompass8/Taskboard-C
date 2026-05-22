using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Injection existante de DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IListRepository, ListRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();

// Services
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IListService, ListService>();
builder.Services.AddScoped<ICardService, CardService>();

// Crypting system for password (BCrypt)
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT Authentification
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

// Controllers
builder.Services.AddControllers();

// Notification Board Hub
builder.Services.AddScoped<IBoardNotificationService, BoardNotificationService>();

// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication(); // Must be BEFORE Authorization
app.UseAuthorization(); // Must be AFTER Authentification
app.MapControllers();
app.MapHub<BoardHub>("/hubs/board");

app.Run();