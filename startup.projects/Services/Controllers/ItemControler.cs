using System.Security.Cryptography;
namespace webapi.controller;

public class ItemController : ApiControllerBase
{
    [HttpPost()]
    [Authorize(Policies.CanCreateItem)]
    public async Task Post()
    {
    }
}
