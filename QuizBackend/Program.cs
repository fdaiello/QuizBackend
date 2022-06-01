using System.Text;
using QuizBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context
//builder.Services.AddDbContext<QuizContext>(opt => opt.UseInMemoryDatabase("Quiz"));
builder.Services.AddDbContext<QuizContext>(opt => opt.UseSqlServer("Server = (localdb)\\MSSQLLocalDB; Integrated Security = true;Initial Catalog=Quiz;uid=QUser;pwd=q123"));
builder.Services.AddDbContext<UserDbContext>(opt => opt.UseSqlServer("Server = (localdb)\\MSSQLLocalDB; Integrated Security = true;Initial Catalog=User;uid=QUser;pwd=q123"));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<UserDbContext>();

// Cors Policy
builder.Services.AddCors(options => options.AddPolicy("Cors", builder => { 
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.AllowAnyOrigin();
}));

// Authentication with Jwts
var singingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("quiz-secret"));

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer( cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = singingKey,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Cors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
