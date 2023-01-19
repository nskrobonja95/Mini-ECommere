using Application.Common.Interfaces;
using Application.Orders;
using Application.Products;
using Moq;

namespace Application.UnitTests.Orders;

public class OrderServiceTests
{

    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        var dbContext = Mock.Of<IApplicationDbContext>();
        var prodService = Mock.Of<IProductService>();
        var idService = Mock.Of<IIdentityService>();
        _orderService = new Mock<OrderService>(dbContext, prodService, idService).Object;
    }

    [Test]
    public void CalculateDiscountTest()
    {
        var discount1 = _orderService.CalculateDiscount("+38163111111");
        var discount2 = _orderService.CalculateDiscount("+38163111112");
        var discount3 = _orderService.CalculateDiscount("+38163111110");
        Assert.That(discount1, Is.EqualTo(0.9));
        Assert.That(discount2, Is.EqualTo(0.8));
        Assert.That(discount3, Is.EqualTo(0.7));
    }

}
