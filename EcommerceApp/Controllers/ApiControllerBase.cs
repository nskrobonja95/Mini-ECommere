using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiControllerBase : ControllerBase
{

    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

}
