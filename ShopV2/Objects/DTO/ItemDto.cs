using System.ComponentModel.DataAnnotations;

namespace ShopV2.Objects.DTO;

public class ItemDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public decimal Price { get; set; }
}
