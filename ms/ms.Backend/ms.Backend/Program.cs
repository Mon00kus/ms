using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ms.Backend.Data;
using ms.Backend.Domain.IRepositories;
using ms.Backend.Domain.IServices;
using ms.Backend.Persistence.Repositories;
using ms.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", 
        new OpenApiInfo { 
            Title = "Register user API", 
            Description ="<p3>Api to Perform CRUD Operations and Register Users in a PostGres Database Previusly Card Serial validation</p3>" ,
            Version = "v1" 
        });
});

builder.Services.AddDbContext<DataContext>(s=>s.UseNpgsql("name=PostGresConnect"));

builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();
builder.Services.AddScoped<IRegisterUserRepository, RegisterUserRepository>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7124/") });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Register Users API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(
    x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials()
);

app.Run();
