using Microsoft.AspNetCore.Mvc;
using ShopV2.Interfaces;
using ShopV2.Objects.DTO;

namespace ShopV2.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItemById(Guid id)
    {
        return Ok(await _itemService.GetItemById(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        return Ok(await _itemService.GetItems());
    }

    [HttpPost]
    public async Task<IActionResult> AddItem(ItemAddDto item)
    {
        Guid guid = await _itemService.AddItem(item);
        return CreatedAtAction(nameof(GetItemById), new { Id = guid }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(Guid id, ItemAddDto _item)
    {
        await _itemService.UpdateItem(id, _item);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        await _itemService.DeleteItem(id);
        return Ok();
    }

    [HttpPost("{id}/Buy")]
    public async Task<IActionResult> BuyItem(Guid id, int quantity)
    {
        decimal price = await _itemService.BuyItem(id, quantity);
        return Ok(new { Price = price });
    }
}
