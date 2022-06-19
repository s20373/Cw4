using System.ComponentModel.DataAnnotations;

namespace Cw4.Models;

public class Product
{
    public int IdProduct { get; set; }
    [MinLength(3)]
    [MaxLength(100)]
    public string Name { get; set; }
    [MinLength(5)]
    [MaxLength(100)]
    public string Description { get; set; }
    public double Price { get; set; }
}