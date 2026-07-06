
using INFRASTRUCTURE.DB;
using Microsoft.AspNetCore.Builder;
using APLICATION.Interfaces;
using APLICATION.Services;
using INFRASTRUCTURE.Repository;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configuración de CORS
builder.Services.AddCors(op =>
{
    op.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});


// Conexión base de datos 
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddSingleton(new DBconexionfactory(connectionString!));

builder.Services.AddScoped<IProvidersRepository, ProvidersRepository>();
builder.Services.AddScoped<ProvidersServices>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<UsersServices>();
builder.Services.AddScoped<IPaymentMethodTypesRepository, PaymentMethodTypesRepository>();
builder.Services.AddScoped<PaymentMethodTypesServices>();




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();


app.UseCors("AllowAll"); // Primero CORS

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "ECCOMERCEDEV");
    s.RoutePrefix = string.Empty;
});

app.UseAuthentication();
app.UseAuthorization(); // Único punto de autorización

app.MapControllers();

app.Run();
