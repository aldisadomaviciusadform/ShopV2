using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Shop;

public class ShopAddDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Address { get; set; }
}
