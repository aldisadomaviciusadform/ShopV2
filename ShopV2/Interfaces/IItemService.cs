using ShopV2.Objects.DTO;

namespace ShopV2.Interfaces
{
    public interface IItemService
    {
        public Task<ItemDto> GetItemById(Guid id);
        public Task<List<ItemDto>> GetItems();
        public Task<Guid> AddItem(ItemAddDto item);
        public Task UpdateItem(Guid id, ItemAddDto item);
        public Task DeleteItem(Guid id);
        public Task<decimal> BuyItem(Guid id, int quantity);
    }
}
