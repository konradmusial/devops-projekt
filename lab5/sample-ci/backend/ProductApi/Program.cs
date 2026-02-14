var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = "InstrumentationKey=cac29210-b7f2-462f-81d8-0e697c969ad1;IngestionEndpoint=https://polandcentral-0.in.applicationinsights.azure.com/;LiveEndpoint=https://polandcentral.livediagnostics.monitor.azure.com/;ApplicationId=d67835ef-9054-412a-a18a-7f7815d9d991";
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

// Simple in-memory product list
var products = new List<Product>
{
    new Product { Id = 1, Name = "Laptop", Price = 999.99m },
    new Product { Id = 2, Name = "Mouse", Price = 29.99m }
};

// API endpoints
app.MapGet("/", () => new { message = "Product API is running", version = "1.0" });

app.MapGet("/products", () => products);

app.MapGet("/products/names", () => products.Select(p => p.Name).ToList());

app.MapGet("/products/prices", () => 
    products.Select(p => p.Price).ToList()
);

app.MapGet("/api/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.MapPost("/api/products", (Product product) =>
{
    product.Id = products.Max(p => p.Id) + 1;
    products.Add(product);
    return Results.Created($"/api/products/{product.Id}", product);
});

app.MapDelete("/api/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product is null) return Results.NotFound();
    
    products.Remove(product);
    return Results.NoContent();
});

app.Run();

// Product model
public record Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
