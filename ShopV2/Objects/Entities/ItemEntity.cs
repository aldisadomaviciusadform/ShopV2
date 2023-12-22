using System.ComponentModel.DataAnnotations.Schema;

namespace ShopV2.Objects.Entities;

//[Table("items")]
public class ItemEntity : BaseEntity
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("price")]
    public decimal Price { get; set; }
}
