using Dapper;
using ShopV2.Interfaces;
using ShopV2.Objects.Entities;
using System.Data;

namespace ShopV2.Repository;

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
        return (await _dbConnection.QuerySingleAsync<ItemEntity>("SELECT * FROM items" +
                                                " WHERE id=@Id AND \"isDeleted\"=false", queryArguments));
    }

    public async Task<IEnumerable<ItemEntity>> Get()
    {
        return await _dbConnection.QueryAsync<ItemEntity>("SELECT * FROM items" +
                                                " WHERE \"isDeleted\"=false");
    }

    public async Task<Guid> Add(ItemEntity item)
    {
        string sql = $"INSERT INTO items" +
                        " (name, price)" +
                        " VALUES (@Name, @Price)" +
                        "RETURNING id";

        return await _dbConnection.ExecuteScalarAsync<Guid>(sql, item);
    }

    public async Task<int> Update(ItemEntity item)
    {
        return await _dbConnection.ExecuteAsync("UPDATE items" +
                                        " SET name=@Name,price=@Price" +
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
}
