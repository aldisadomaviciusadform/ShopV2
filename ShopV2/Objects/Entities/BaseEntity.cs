using System.ComponentModel.DataAnnotations.Schema;

namespace ShopV2.Objects.Entities
{
    public class BaseEntity
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
    }
}
