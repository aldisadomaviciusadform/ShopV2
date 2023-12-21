using Microsoft.EntityFrameworkCore;
using ShopV2.Objects.Entities;

namespace ShopV2.Contexts;

public class DataContext : DbContext
{
    public DbSet<ItemEntity> Items { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
}
