using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.User;

public class BuyDto
{
    [Required]
    public Guid ShopId { get; set; }

    [Required]
    public Guid ItemId { get; set; }

    [Required]
    public uint Quantity { get; set; }
}
