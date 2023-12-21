using ShopV2.Objects.Entities;

namespace ShopV2.Interfaces
{
    public interface IItemRepository
    {
        public Task<ItemEntity?> Get(Guid id);

        public Task<IEnumerable<ItemEntity>> Get();

        public Task<Guid> AddItem(ItemEntity item);

        public Task<int> UpdateItem(ItemEntity item);
        public Task DeleteItem(Guid id);
    }
}
