﻿using Domain.Entities;

namespace Domain.Interfaces;

public interface IItemRepository
{
    public Task<ItemEntity?> Get(Guid id);

    public Task<IEnumerable<ItemEntity>> Get();

    public Task<Guid> Add(ItemEntity item);

    public Task<int> Update(ItemEntity item);

    public Task Delete(Guid id);

    public void BuyItem(BuyItemEntity buy);
}