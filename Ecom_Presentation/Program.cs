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
//Acuerdense de instalar Swashbuckle.AspNetCore
// Soporte para Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ECommerce", Version = "v1" });
});

var app = builder.Build();



// Swagger )
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce V1");
        s.RoutePrefix = string.Empty; // Swagger en la raíz 
    });
}

app.UseHttpsRedirection();

//CORS 
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();