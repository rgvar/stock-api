using System.ComponentModel.DataAnnotations;

namespace StockMaster.Dtos;
public class UpdateProductDto
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public ICollection<CategorySimpleDto> Categories { get; set; } = [];
}


