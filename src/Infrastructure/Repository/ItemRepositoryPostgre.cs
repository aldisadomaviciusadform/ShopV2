﻿using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.Repository;

public class ItemRepositoryPostgre : IItemRepository
{
    private readonly IDbConnection _dbConnection;
    public ItemRepositoryPostgre(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ItemEntity?> Get(Guid id)
    {
        var queryArguments = new
        {
            Id = id
        };

        return await _dbConnection.QuerySingleAsync<ItemEntity>("SELECT * FROM items" +
                                                " WHERE id=@Id AND \"isDeleted\"=false", queryArguments);
    }

    public async Task<IEnumerable<ItemEntity>> Get()
    {
        return await _dbConnection.QueryAsync<ItemEntity>("SELECT * FROM items" +
                                                " WHERE \"isDeleted\"=false");
    }

    public async Task<Guid> Add(ItemEntity item)
    {
        string sql = $"INSERT INTO items" +
                        " (name, price, shopId)" +
                        " VALUES (@Name, @Price, @ShopId)" +
                        "RETURNING id";

        return await _dbConnection.ExecuteScalarAsync<Guid>(sql, item);
    }

    public async Task<int> Update(ItemEntity item)
    {
        return await _dbConnection.ExecuteAsync("UPDATE items" +
                                        " SET name=@Name,price=@Price,\"shopId\"=@ShopId" +
                                        " WHERE id=@Id AND \"isDeleted\"=false", item);
    }

    public async Task Delete(Guid id)
    {
        var queryArguments = new
        {
            Id = id
        };

        await _dbConnection.ExecuteAsync("UPDATE items" +
                                        " SET \"isDeleted\"=true" +
                                        " WHERE id=@Id AND \"isDeleted\"=false", queryArguments);
    }

    public async void BuyItem(BuyItemEntity buy)
    {
        string sql = $"INSERT INTO \"boughtItems\"" +
                        " (shopId, userId, itemId, price)" +
                        " VALUES (@ShopId, @UserId, @ItemId, @Price)" +
                        "RETURNING id";

        await _dbConnection.ExecuteScalarAsync(sql, buy);
    }
}
