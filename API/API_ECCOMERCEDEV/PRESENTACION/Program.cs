
using APLICATION.Interfaces;
using APLICATION.Services;
using INFRASTRUCTURE.DB;
using INFRASTRUCTURE.JWT;
using INFRASTRUCTURE.Repository;
using Microsoft.AspNetCore.Builder;


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
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IProvidersRepository, ProvidersRepository>();
builder.Services.AddScoped<ProvidersServices>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<UsersServices>();
builder.Services.AddScoped<IPaymentMethodTypesRepository, PaymentMethodTypesRepository>();
builder.Services.AddScoped<PaymentMethodTypesServices>();
builder.Services.AddScoped<ISubCategoriesRepository, SubCategoriesRepository>();
builder.Services.AddScoped<SubCategoriesServices>();
builder.Services.AddScoped<ISegmentsRepository, SegmentsRepository>();
builder.Services.AddScoped<SegmentsServices>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<ProductsServices>();
builder.Services.AddScoped<IPaymentOrdersRepository, PaymentOrdersRepository>();
builder.Services.AddScoped<PaymentOrdersServices>();
builder.Services.AddScoped<IStockMovementsRepository, StockMovementsRepository>();
builder.Services.AddScoped<StockMovementsServices>();
builder.Services.AddScoped<ICurrenciesRepository, CurrenciesRepository>();
builder.Services.AddScoped<CurrenciesServices>();
builder.Services.AddScoped<IUserPaymentMethodsRepository, UserPaymentMethodsRepository>();
builder.Services.AddScoped<UserPaymentMethodsServices>();
builder.Services.AddScoped<IMarksRepository, MarksRepository>();
builder.Services.AddScoped<MarksServices>();
builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();
builder.Services.AddScoped<UserAddressServices>();
builder.Services.AddScoped<IPaymentOrderDetailsRepository, PaymentOrderDetailsRepository>();
builder.Services.AddScoped<PaymentOrderDetailsServices>();
builder.Services.AddScoped<IStockMovementDetailsRepository, StockMovementDetailsRepository>();
builder.Services.AddScoped<StockMovementDetailsServices>();








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
