using Cw4.Models;
using Cw4.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw4.Controllers;

[ApiController]
[Route("[controller]")]
public class WarehousesController : ControllerBase
{

    private WarehouseService _warehouseService = new WarehouseService();

    [HttpPost]
    public IActionResult AddProduct(ProductWarehouse productWarehouse)
    {
        var productIdExists = _warehouseService.CheckIfExists(productWarehouse.IdProduct, "IdProduct", "Product");
        var warehouseIdExists = _warehouseService.CheckIfExists(productWarehouse.IdWarehouse, "IdWarehouse", "Warehouse");
        var amountGtZero = productWarehouse.Amount > 0;
        
        if (!productIdExists || !warehouseIdExists) { return NotFound("Couldn't find matching records"); }
        if (!amountGtZero) { return NotFound("Amount must be greater than 0"); }

        var order = _warehouseService.GetOrder(productWarehouse);
        if (order.FulfilledAt is null) { return NotFound("Couldn't find matching order"); }

        var orderAlreadyFulfilled = _warehouseService.CheckIfExists(order.IdOrder, "IdOrder", "Product_Warehouse");
        if (orderAlreadyFulfilled) { return NotFound("Order was already fulfilled"); }

        _warehouseService.UpdateOrder(order.IdOrder);
        
        return Ok(_warehouseService.AddProduct(productWarehouse, order.IdOrder));
    }
    
}