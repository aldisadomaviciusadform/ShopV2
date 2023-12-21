using System.ComponentModel.DataAnnotations.Schema;

namespace ShopV2.Objects.Entities;

//[Table("items")]
public class ItemEntity
{
    [Column("created")]
    public DateTime Created { get; set; }

    [Column("createdBy")]
    public string CreatedBy { get; set; } = string.Empty;

    [Column("isDeleted")]
    public bool IsDeleted { get; set; }

    [Column("modified")]
    public DateTime? Modified { get; set; }

    [Column("modifiedBy")]
    public string? ModifiedBy { get; set; }


    [Column("id")]
    public Guid Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("price")]
    public decimal Price { get; set; }
}
