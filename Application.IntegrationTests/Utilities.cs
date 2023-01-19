using Domain.Models;
using Infrastructure.Persistence;

namespace Application.IntegrationTests;

public static class Utilities
{

    #region snippet1
    public static void InitializeDbForTests(ApplicationDbContext db)
    {
        db.MeasureUnits.AddRange(GetMeasureUnits());
        db.Products.AddRange(GetProducts(db.MeasureUnits.FirstOrDefault()!.Id));
        List<ProductPrice> productPrices = new List<ProductPrice>();
        foreach (var product in db.Products)
        {
            var prodPrice = new ProductPrice()
            {
                ProductId = product.Id,
                UnitPrice = 100,
                ValidFrom = DateTime.Now
            };
            productPrices.Add(prodPrice);
        }
        db.ProductPrices.AddRange(productPrices);
        db.SaveChanges();
    }

    public static void ReinitializeDbForTests(ApplicationDbContext db)
    {
        db.Products.RemoveRange(db.Products);
        InitializeDbForTests(db);
    }

    public static List<Product> GetProducts(Guid measureUnit)
    {
        return new List<Product>()
            {
                new Product { Name = "Product1", Quantity = 50.0, MeasureUnitId = measureUnit },
                new Product { Name = "Product2", Quantity = 150.5, MeasureUnitId = measureUnit },
                new Product { Name = "Product3", Quantity = 20, MeasureUnitId = measureUnit }
            };
    }
 
    public static List<MeasureUnit> GetMeasureUnits()
    {
        return new List<MeasureUnit>()
            {
                new MeasureUnit() { Name = "Pieces", ShortName = "pcs" },
                new MeasureUnit() { Name = "Kilograms", ShortName = "kg" }
            };
    }

    public static void AddCartItemsForCustomer(ApplicationDbContext db, Guid customerId)
    {
        var product = db.Products.FirstOrDefault();
        var cartItems = new List<CartItem>()
            {
                new CartItem() { CustomerId = customerId, ProductId = product!.Id, NumberOfUnits = 20},
                new CartItem() { CustomerId = customerId, ProductId = product.Id, NumberOfUnits = 100}
        };
        db.CartItems.AddRange(cartItems);
        db.SaveChanges();
    }

    public static void ClearCartItemsForCustomer(ApplicationDbContext db, Guid customerId)
    {
        db.CartItems.RemoveRange(db.CartItems.Where(c => c.CustomerId == customerId).ToList());
        db.SaveChanges();
    }


    #endregion

    public const string SCHEMENAME = "Test";


}
