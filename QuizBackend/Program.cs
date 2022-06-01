using System.Text;
using QuizBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

// Configuration
var envName = Environment.GetEnvironmentVariable("NAME");           // This is not working. Todo: How to setup appsettings.Development ?
var cfBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var Configuration = cfBuilder.Build();

// App builder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context
//builder.Services.AddDbContext<QuizContext>(opt => opt.UseInMemoryDatabase("Quiz"));
var quizConnString = Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<QuizContext>(opt => opt.UseSqlServer(quizConnString));

var userConnString = Configuration.GetConnectionString("UserDbConnection");
builder.Services.AddDbContext<UserDbContext>(opt => opt.UseSqlServer(userConnString));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<UserDbContext>();

// Cors Policy
builder.Services.AddCors(options => options.AddPolicy("Cors", builder => { 
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.AllowAnyOrigin();
}));

// Authentication with Jwts
var secret = Configuration.GetValue<string>($"JwtSecret");
var singingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret"));

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
