using Ecom_Aplication.Interfaces;
using Ecom_Aplication.Services;
using Ecom_Domain;
using Ecom_Infraestructure.Database;
using Ecom_Infraestructure.Repository;
using modu.application.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddSingleton(new DB_conection(connectionString!));

builder.Services.AddCors(op =>
{
    op.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ECommerce", Version = "v1" });
});


// AttributeProducts
builder.Services.AddScoped<IAttributeProductsRepository, AttributeProducts_Repository>();
builder.Services.AddScoped<AttributeProducts_Services>();

// AttributeProductVariables
builder.Services.AddScoped<IAttributeProductVariablesRepository, AttributeProductVariables_Repository>();
builder.Services.AddScoped<AttributeProductVariables_Services>();

// AttributesType
builder.Services.AddScoped<IAttributesType, AttributesType_Repository>(); 
builder.Services.AddScoped<AttributesType_Services>();

// CartDetails
builder.Services.AddScoped<ICartDetails, CartDetail_Repository>();
builder.Services.AddScoped<CartDetail_Services>();

// Carts
builder.Services.AddScoped<ICartsRepository, Carts_Repository>();
builder.Services.AddScoped<Carts_Services>();

// Category
builder.Services.AddScoped<ICategoryRepository, Category_Repository>(); 
builder.Services.AddScoped<Category_Services>();

// Currencies
builder.Services.AddScoped<ICurrenciesRepository, Currencies_Repository>();
builder.Services.AddScoped<Currencies_Services>();

// MarkByProviders
builder.Services.AddScoped<IMarkByProviders, MarkByProviders_Repository>();
builder.Services.AddScoped<MarkByProviders_Services>();

// Mark (Marcas)
builder.Services.AddScoped<IMarkRepository, Mark_Repository>();
builder.Services.AddScoped<Mark_Services>();

// PaymentMethodTypes
builder.Services.AddScoped<IPaymentMethodTypes, PaymentMethodTypes_Repository>(); // Ojo: en tus archivos tenías esta como _Services.cs en Repository, si es una interfaz cámbiala por su repositorio correspondiente.
builder.Services.AddScoped<PaymentMethodTypes_Services>();

// PaymentOrderDetails
builder.Services.AddScoped<IPaymentOrderDetails, PaymentOrderDetails_Repository>();
builder.Services.AddScoped<PaymentOrderDetails>(); // Respetando el typo de tu archivo "_Sevices"

// PaymentOrders
builder.Services.AddScoped<IPaymentOrders, PaymentOrders_Repository>();
builder.Services.AddScoped<PaymentOrders_Services>();

// PriceHistory
builder.Services.AddScoped<IPriceHistory, PriceHistory_Repository>(); // Respetando el typo "IPrinceHistory" de tu archivo
builder.Services.AddScoped<PriceHistory_Services>();

// ProductIdentificators
builder.Services.AddScoped<IProductIdentificators, ProductIdentificators_Repository>();
builder.Services.AddScoped<ProductIdentificators_Services>();

// ProductImages
builder.Services.AddScoped<IProductImagesRepository, ProductImages_Repository>();
builder.Services.AddScoped<ProductImages_Services>();

// Products
builder.Services.AddScoped<IProductsRepository, Products_Repository>();
builder.Services.AddScoped<Products_Services>();

// ProductVariables
builder.Services.AddScoped<IProductVariablesRepository, ProductVariables_Repository>();
builder.Services.AddScoped<ProductVariables_Services>();

// ProductVariableTypes
builder.Services.AddScoped<IProductVariableTypesRepository, ProductVariableTypes_Repository>();
builder.Services.AddScoped<ProductVariableTypes_Services>();

// Provider
builder.Services.AddScoped<IProviderRepository, Provider_Repository>();
builder.Services.AddScoped<Providers_Services>();

// Segment
builder.Services.AddScoped<ISegmentRepository, Segment_Repository>();
builder.Services.AddScoped<Segments_Services>();

// Status
builder.Services.AddScoped<IStatus, Status_Repository>();
builder.Services.AddScoped<Status_Services>();

// StockMovementDetails
builder.Services.AddScoped<IStockMovementDetails, StockMovementDetails_Repository>();
builder.Services.AddScoped<StockMovementDetails_Services>();

// StockMovements
builder.Services.AddScoped<IStockMovements, StockMovements_Repository>();
builder.Services.AddScoped<StockMovements_Services>();

// StockMovementTypes
builder.Services.AddScoped<IStockMovementTypes, StockMovementTypes_Repository>();
builder.Services.AddScoped<StockMovementTypes_Services>();

// Stocks
builder.Services.AddScoped<IStocksRepository, Stocks_Repository>();
builder.Services.AddScoped<Stocks_Services>();

// SubCategory
builder.Services.AddScoped<ISubCategoryRepository, SubCategory_Repository>();
builder.Services.AddScoped<SubCategory_Services>();

// User
builder.Services.AddScoped<IUserRepository, User_Repository>();
builder.Services.AddScoped<User_Services>();

// UserAddress
builder.Services.AddScoped<IUserAddressRepository, UserAddress_Repository>();
builder.Services.AddScoped<UserAddress_Services>();

// UserPaymentMethods
builder.Services.AddScoped<IUserPaymentMethods, UserPaymentMethods_Repository>();
builder.Services.AddScoped<UserPaymentMethods_Services>();

// ==========================================

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce V1");
        s.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();