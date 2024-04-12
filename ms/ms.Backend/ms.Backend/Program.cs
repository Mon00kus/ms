using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ms.Backend.Data;
using ms.Backend.Domain.IRepositories;
using ms.Backend.Domain.IServices;
using ms.Backend.Persistence.Repositories;
using ms.Backend.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Builder;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var _userName = configuration["Loggin:Username"];
var _password = configuration["Loggin:Password"];
var _issuer = configuration["JwtBearer:Issuer"];
var _audience = configuration["JwtBearer:Audience"];
var _secretKey = configuration["JwtBearer:SecretKey"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", 
        new OpenApiInfo { 
            Title = "Register user API", 
            Description ="Api to Perform CRUD Operations and Register Users in a PostGres Database Previusly Card Serial validation" ,
            Version = "v1" 
        });
    
    c.AddSecurityDefinition("Bearer", 
        new OpenApiSecurityScheme  {
            Name = "Autorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Ingrese su info..."
        });

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<DataContext>(s=>s.UseNpgsql("name=PostGresConnect"));

builder.Services.AddScoped<IRegisterUserService, RegisterUserService>();
builder.Services.AddScoped<IRegisterUserRepository, RegisterUserRepository>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7124/") });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Establece los parámetros de validación del token
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JwtBearer:Issuer"],
            ValidAudience = configuration["JwtBearer:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtBearer:SecretKey"]!)),
            ClockSkew = TimeSpan.Zero
        };

        // Configura los eventos de JwtBearer para personalizar la respuesta en desafíos
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                // Evita el envío del header WWW-Authenticate
                context.HandleResponse();

                // Personaliza la respuesta
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                // Escribe el contenido de la respuesta JSON personalizada
                var result = JsonSerializer.Serialize(new { error = "Usted no está autorizado para acceder a este recurso." });
                return context.Response.WriteAsync(result);
            }
        };
    });


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.Use(async (context, next) =>
    {

        string authHeader = context.Request.Headers["Authorization"]!;

        if ( authHeader != null && authHeader.StartsWith("Basic") )
        {
            // Decode the Base64 string
            var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
            
            var username = decodedUsernamePassword.Split(':', 2)[0];
            var password = decodedUsernamePassword.Split(':', 2)[1];
            
            if (username == _userName && password == _password)
            {
                await next();
                return;
            }
        }

        if ( authHeader != null && authHeader.StartsWith("Bearer"))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();

            // Configura las opciones de validación de tokens
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey!)), // Asegúrate de que _secretKey es tu clave secreta para firmar el token
                ValidateIssuer = true,
                ValidIssuer = _issuer, // El emisor del token
                ValidateAudience = true,
                ValidAudience = _audience, // La audiencia del token
                ValidateLifetime = true, // Valida la expiración
                ClockSkew = TimeSpan.Zero // La tolerancia de tiempo para la expiración del token
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Intenta validar el token
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // Si llega aquí, el token es válido
                await next();
                return;
            }
            catch (SecurityTokenException)
            {
                // El token no es válido
                context.Response.StatusCode = 403; // el servidor ha entendido la solicitud pero se niega a authorizarla Forbidden o puedes usar 401 Unauthorized
            }
            catch (Exception)
            {
                // Otro tipo de error
                context.Response.StatusCode = 500; // Internal Server Error
            }
        }

        // Return authentication type (causes browser to show login dialog)        
        context.Response.Headers["WWW-Authenticate"] = "Basic";
        context.Response.StatusCode = 401; //Unauthorized
        return;

    });

    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Register Users API v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();

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
