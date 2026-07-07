using warehouse_management_api_NourM.Services;

var builder = WebApplication.CreateBuilder(args);

// Register controllers
builder.Services.AddControllers();

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register the Supplier Service
builder.Services.AddSingleton<SupplierService>();
builder.Services.AddSingleton<ProductService>();
var app = builder.Build();
// To enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// To map controller endpoints
app.MapControllers();

app.Run();