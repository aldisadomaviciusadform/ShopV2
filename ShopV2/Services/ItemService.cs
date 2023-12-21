using ShopV2.Exceptions;
using ShopV2.Interfaces;
using ShopV2.Objects.DTO;
using ShopV2.Objects.Entities;

namespace ShopV2.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<ItemDto> GetItemById(Guid id)
        {
            ItemEntity item = await _itemRepository.Get(id) ?? throw new EntityNotFoundException("Item not found in DB");

            ItemDto itemDto = new()
            {
                Id = id,
                Name = item.Name,
                Price = item.Price,
            };

            return itemDto;
        }

        public async Task<List<ItemDto>> GetItems()
        {
            List<ItemDto> items = [];
            IEnumerable<ItemEntity> itemEntities = await _itemRepository.Get();

            if (itemEntities == null)
                throw new EntityNotFoundException("Item not found in DB");

            items = itemEntities.Select(i => new ItemDto()
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
            }).ToList();

            return items;
        }

        public async Task<Guid> AddItem(ItemAddDto item)
        {
            ItemEntity itemEntity = new()
            {
                Name = item.Name,
                Price = item.Price,
            };

            return await _itemRepository.AddItem(itemEntity);
        }

        public async Task UpdateItem(Guid id, ItemAddDto item)
        {
            await GetItemById(id);

            ItemEntity itemEntity = new()
            {
                Id = id,
                Name = item.Name,
                Price = item.Price,
            };

            int result = await _itemRepository.UpdateItem(itemEntity);

            if (result == 0)
                throw new EntityNotFoundException("Item not found in DB");
            else if (result > 1)
                throw new InvalidOperationException("Update was performed on multiple rows");
        }

        public async Task DeleteItem(Guid id)
        {
            ItemDto items = await GetItemById(id);

            await _itemRepository.DeleteItem(id);
        }

        public async Task<decimal> BuyItem(Guid id, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Amount must be more than 0");

            decimal netAmount;
            if (quantity > 20)
                netAmount = 0.8m;
            else if (quantity > 10)
                netAmount = 0.9m;
            else
                netAmount = 1.0m;

            ItemDto items = await GetItemById(id);

            return quantity * items.Price * netAmount;
        }
    }
}
