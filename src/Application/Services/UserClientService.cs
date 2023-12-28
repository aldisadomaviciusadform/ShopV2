using Application.DTO.Item;
using Application.DTO.User;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public class UserClientService
{
    private readonly IGeneralClient user;
    private readonly ItemService _itemService;

    public UserClientService(IGeneralClient generalExternalAPI, ItemService itemService, ShopService shopService)
    {
        user = generalExternalAPI;
        _itemService = itemService;
    }

    public async Task<UserDto> Get(int id)
    {
        var result = await user.Get<UserDto>("users", id);

        if (!result.IsSuccessful)
            throw new NotFoundException("User not found");

        return result.Data!;
    }

    public async Task<List<UserDto>> Get()
    {
        var result = await user.Get<IEnumerable<UserDto>>("users");

        if (!result.IsSuccessful)
            throw new NotFoundException("User not found");

        return result.Data!.ToList();
    }

    public async Task<UserDto> Add(UserAddDto item)
    {
        var result = await user.Add<UserAddDto, UserDto>("users", item);

        if (!result.IsSuccessful)
            throw new NotFoundException("User add failed");

        return result.Data!;
    }

    public async Task Buy(int id, BuyDto buy)
    {
        await Get(id);
        ItemDto item = await _itemService.Get(buy.ItemId);

        if (item.ShopId != buy.ShopId)
            throw new NotFoundException("Item not found in this shop");

        decimal result = _itemService.GetItemsPrice(item.Price, buy.Quantity);
        decimal result = await _itemService.Buy(buy.ItemId, buy.Quantity);
    }
}
