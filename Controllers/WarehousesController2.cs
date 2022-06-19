using Cw4.Models;
using Cw4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw4.Controllers;

[ApiController]
[Route("[controller]")]
public class WarehousesController2 : ControllerBase
{
    
    private WarehouseService2 _warehouseService2 = new WarehouseService2();
    
    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductWarehouse productWarehouse)
    {
        int id;
        try
        {
            id = await _warehouseService2.AddProduct(productWarehouse);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
        return Ok(id);
    }
}