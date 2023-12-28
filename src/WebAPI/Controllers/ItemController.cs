using Application.DTO;
using Application.DTO.Item;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ItemController : ControllerBase
{
    private readonly ItemService _itemService;

    public ItemController(ItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _itemService.Get());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _itemService.Get(id));
    }

    [HttpPost]
    public async Task<IActionResult> Add(ItemAddDto item)
    {
        Guid guid = await _itemService.Add(item);
        return CreatedAtAction(nameof(Get), new { Id = guid }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, ItemAddDto _item)
    {
        await _itemService.Update(id, _item);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _itemService.Delete(id);
        return Ok();
    }

    [HttpPut("{id}/addToShop")]
    public async Task<IActionResult> AddToShop(Guid id, GuidDto shop)
    {
        await _itemService.AddToShop(id, shop);
        return Ok();
    }
}