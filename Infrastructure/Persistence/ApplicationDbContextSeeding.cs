
using Domain.Models;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public class ApplicationDbContextSeeding
{

    private readonly ILogger<ApplicationDbContextSeeding> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    public ApplicationDbContextSeeding(ILogger<ApplicationDbContextSeeding> logger, ApplicationDbContext context,
        UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole("Administrator");
        var customerRole = new IdentityRole("Customer");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }
        if (_roleManager.Roles.All(r => r.Name != customerRole.Name))
        {
            await _roleManager.CreateAsync(customerRole);
        }

            // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost", SecurityStamp = Guid.NewGuid().ToString()};
        var ccustomer1 = new ApplicationUser { UserName = "Customer1", Email = "test1@mail.com", PhoneNumber = "+38163111111", SecurityStamp = Guid.NewGuid().ToString() };
        var ccustomer2 = new ApplicationUser { UserName = "Customer2", Email = "test1@mail.com", PhoneNumber = "+38163111111", SecurityStamp = Guid.NewGuid().ToString() };
        var ccustomer3 = new ApplicationUser { UserName = "Customer3", Email = "test1@mail.com", PhoneNumber = "+38163111111", SecurityStamp = Guid.NewGuid().ToString() };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
            await _userManager.CreateAsync(ccustomer1, "Customer1!");
            await _userManager.CreateAsync(ccustomer2, "Customer2!");
            await _userManager.CreateAsync(ccustomer3, "Customer3!");
            if (!string.IsNullOrWhiteSpace(customerRole.Name))
            {
                await _userManager.AddToRolesAsync(ccustomer1, new[] { customerRole.Name });
                await _userManager.AddToRolesAsync(ccustomer2, new[] { customerRole.Name });
                await _userManager.AddToRolesAsync(ccustomer3, new[] { customerRole.Name });
            }
        }

        var pieces = new MeasureUnit() { Name = "Pieces", ShortName = "pcs" };
        var kilos = new MeasureUnit() { Name = "Kilograms", ShortName = "kg" };

        if (!_context.MeasureUnits.Any())
        {
            _context.MeasureUnits.AddRange(new List<MeasureUnit> { pieces, kilos });
        }

        // Default users
        var product1 = new Product { Name = "Product1", Quantity = 50.0, MeasureUnitId = pieces.Id };
        var product2 = new Product { Name = "Product2", Quantity = 150.5, MeasureUnitId = kilos.Id };
        var product3 = new Product { Name = "Product3", Quantity = 20, MeasureUnitId = pieces.Id };


        if (!_context.Products.Any())
        {
            _context.Products.AddRange(new List<Product> { product1, product2, product3 });
        }

        var price1 = new ProductPrice() { UnitPrice = 40, ValidFrom = new DateTime(), ProductId = product1.Id };
        var price2 = new ProductPrice() { UnitPrice = 40, ValidFrom = new DateTime(), ProductId = product2.Id };
        var price3 = new ProductPrice() { UnitPrice = 40, ValidFrom = new DateTime(), ProductId = product3.Id };

        if (!_context.ProductPrices.Any())
        {
            _context.ProductPrices.AddRange(new List<ProductPrice> { price1, price2, price3 });
        }

        await _context.SaveChangesAsync(CancellationToken.None);
    }

}
