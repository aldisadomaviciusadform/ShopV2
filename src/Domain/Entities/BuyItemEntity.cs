﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class BuyItemEntity:BaseEntity
{
    public Guid ShopId {  get; set; }
    public int UserId { get; set; }
    public Guid ItemId { get; set; }
    public decimal Price { get; set; }

}
