using Application.Products;
using Application.Products.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers;

[Authorize]
public class ProductsController : ApiControllerBase
{

    [HttpGet]
    public Task<List<ProductDto>> GetProducts()
    {
        return Mediator.Send(new GetProductsQuery());
    }

}
