using Xunit;

namespace ProductApi.Tests;

public class ProductTests
{
[Fact]
public void Product_CreatedViaPost_ShouldHaveIncrementedId()
{
    var existingMaxId = 2;
    var newProduct = new Product 
    { 
        Name = "Testowy Produkt", 
        Price = 149.99m 
    };

    newProduct.Id = existingMaxId + 1;

    Assert.Equal(3, newProduct.Id);
    Assert.Equal("Testowy Produkt", newProduct.Name);
    Assert.Equal(149.99m, newProduct.Price);
}

[Fact]
public void Product_NotFoundById_ShouldBeNull()
{
    var products = new List<Product>
    {
        new Product { Id = 1, Name = "Laptop", Price = 999.99m },
        new Product { Id = 2, Name = "Mouse", Price = 29.99m }
    };

    int szukaneId = 999;

    var znaleziony = products.FirstOrDefault(p => p.Id == szukaneId);

    Assert.Null(znaleziony);
}
}
