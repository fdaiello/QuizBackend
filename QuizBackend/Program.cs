
using QuizBackend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context
//builder.Services.AddDbContext<QuizContext>(opt => opt.UseInMemoryDatabase("Quiz"));
builder.Services.AddDbContext<QuizContext>(opt => opt.UseSqlServer("Server = (localdb)\\MSSQLLocalDB; Integrated Security = true;Initial Catalog=Quiz;uid=QUser;pwd=q123"));

// Cors Policy
builder.Services.AddCors(options => options.AddPolicy("Cors", builder => { 
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.AllowAnyOrigin();
}));

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
