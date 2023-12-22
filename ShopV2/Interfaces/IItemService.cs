using ShopV2.Objects.DTO;

namespace ShopV2.Interfaces;

public interface IItemService
{
    public Task<ItemDto> Get(Guid id);
    public Task<List<ItemDto>> Get();
    public Task<Guid> Add(ItemAddDto item);
    public Task Update(Guid id, ItemAddDto item);
    public Task Delete(Guid id);
    public Task<decimal> Buy(Guid id, int quantity);
}
