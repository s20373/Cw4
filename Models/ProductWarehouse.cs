using System.ComponentModel.DataAnnotations;

namespace Cw4.Models;

public class ProductWarehouse
{
    public int IdProductWarehouse { get; set; }
    [Required]
    public int IdWarehouse { get; set; }
    [Required]
    public int IdProduct { get; set; }
    public int IdOrder { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int Amount { get; set; }
    public double Price { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
}