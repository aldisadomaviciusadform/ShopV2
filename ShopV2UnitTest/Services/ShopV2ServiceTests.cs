using FluentAssertions;
using Moq;
using ShopV2.Exceptions;
using ShopV2.Interfaces;
using ShopV2.Objects.DTO;
using ShopV2.Objects.Entities;
using ShopV2.Repository;
using ShopV2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopV2UnitTest.Services;

public class ShopV2ServiceTests
{
    [Theory]
    [InlineData("7b685ea3-4a94-4c2a-8ee7-dfe8b1b407c1")]
    [InlineData("e4fc2fb7-2d52-482c-a5eb-b9a50df09e6e")]
    [InlineData("a1475dd7-f8f9-4b16-8917-c2a64d76a4e4")]
    [InlineData("1e7a76bb-3b91-4eef-9ab5-ae991d558442")]
    [InlineData("fcdfedf4-bb79-4e29-8e22-d399a27f927d")]
    [InlineData("41aa94c9-aa14-4d90-8622-628b778f15e3")]
    [InlineData("c715fbcf-6c3f-4c4d-a4d5-754a06d3d189")]
    [InlineData("5a4b2cf3-c80a-4e87-8e50-cd8d117f22a9")]
    [InlineData("dc59a1e7-1949-4f0f-b13c-96ce0d12e2b5")]
    [InlineData("7f64e01e-82d5-41cc-8707-8eeff43e550d")]
    public async Task Getid_GivenValidId_ReturnsDTO(Guid id)
    {
        //Arrange
        var itemRepository = new Mock<IItemRepository>();
        itemRepository.Setup(m => m.Get(id))
                        .ReturnsAsync(new ItemEntity { Id = id });

        ItemService itemService = new (itemRepository.Object);

        //Act
        ItemDto result = await itemService.Get(id);

        //Assert
        result.Id.Should().Be(id);
    }

    [Fact]
    public async Task Getid_GivenInvalidId_ThrowNotFoundException()
    {        
        // Arrange
        Guid id = new();
        var itemRepository = new Mock<IItemRepository>();
        itemRepository.Setup(m => m.Get(id))
                        .Returns(Task.FromResult<ItemEntity?>(null));

        ItemService itemService = new (itemRepository.Object);

        // Act Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await itemService.Get(id));
    }

    [Fact]
    public async Task Get_GivenValidId_ReturnsDTO()
    {
        //Arrange
        var itemRepository = new Mock<IItemRepository>();
        itemRepository.Setup(m => m.Get())
                        .ReturnsAsync(new List<ItemEntity> 
                        { 
                            new ItemEntity {Id = new Guid()},
                            new ItemEntity {Id = new Guid()}
                        });

        ItemService itemService = new(itemRepository.Object);

        //Act
        var result = await itemService.Get();

        //Assert
        result.Count.Should().Be(2);
    }

    [Fact]
    public async Task Get_GivenInvalidId_ThrowNotFoundException()
    {
        // Arrange
        var itemRepository = new Mock<IItemRepository>();
        itemRepository.Setup(m => m.Get())
                        .Returns(Task.FromResult<IEnumerable<ItemEntity>>(null!));

        ItemService itemService = new (itemRepository.Object);

        // Act Assert
        await itemService.Invoking(x => x.Get())
                    .Should().ThrowAsync<NotFoundException>();

//        await Assert.ThrowsAsync<NotFoundException>(async () => await itemService.Get());
    }

    [Theory]
    [InlineData("7b685ea3-4a94-4c2a-8ee7-dfe8b1b407c1")]
    [InlineData("e4fc2fb7-2d52-482c-a5eb-b9a50df09e6e")]
    [InlineData("a1475dd7-f8f9-4b16-8917-c2a64d76a4e4")]
    [InlineData("1e7a76bb-3b91-4eef-9ab5-ae991d558442")]
    [InlineData("fcdfedf4-bb79-4e29-8e22-d399a27f927d")]
    [InlineData("41aa94c9-aa14-4d90-8622-628b778f15e3")]
    [InlineData("c715fbcf-6c3f-4c4d-a4d5-754a06d3d189")]
    [InlineData("5a4b2cf3-c80a-4e87-8e50-cd8d117f22a9")]
    [InlineData("dc59a1e7-1949-4f0f-b13c-96ce0d12e2b5")]
    [InlineData("7f64e01e-82d5-41cc-8707-8eeff43e550d")]
    public async Task Add_GivenValidId_ReturnsGuid(Guid id)
    {
        //Arrange
        var itemRepository = new Mock<IItemRepository>();
        itemRepository.Setup(m => m.Add(It.IsAny<ItemEntity>()))
                        .ReturnsAsync(id);

        ItemService itemService = new(itemRepository.Object);

        //Act
        Guid result = await itemService.Add(new ItemAddDto {Name = "aa", Price = 5});

        //Assert
        result.Should().Be(id);
    }

    [Theory]
    [InlineData("7b685ea3-4a94-4c2a-8ee7-dfe8b1b407c1", "aa", 5.5)]
    [InlineData("e4fc2fb7-2d52-482c-a5eb-b9a50df09e6e", "aab", 5.5)]
    [InlineData("a1475dd7-f8f9-4b16-8917-c2a64d76a4e4", "aad", 5.5)]
    [InlineData("1e7a76bb-3b91-4eef-9ab5-ae991d558442", "aaq", 5.5)]
    [InlineData("fcdfedf4-bb79-4e29-8e22-d399a27f927d", "aaer", 5.5)]
    [InlineData("41aa94c9-aa14-4d90-8622-628b778f15e3", "auoia", 5.5)]
    [InlineData("c715fbcf-6c3f-4c4d-a4d5-754a06d3d189", "apa", 5.5)]
    [InlineData("5a4b2cf3-c80a-4e87-8e50-cd8d117f22a9", "aabnn", 5.5)]
    [InlineData("dc59a1e7-1949-4f0f-b13c-96ce0d12e2b5", "aahkui", 5.5)]
    [InlineData("7f64e01e-82d5-41cc-8707-8eeff43e550d", "aauuq", 5.5)]
    public async Task Update_ReturnsSucces(Guid id, string name, decimal price)
    {
        //Arrange
        var itemRepository = new Mock<IItemRepository>();
        itemRepository.Setup(m => m.Update(It.IsAny<ItemEntity>()))
                             .ReturnsAsync(1);

        itemRepository.Setup(m => m.Get(id))
                             .ReturnsAsync(new ItemEntity {Id = id });

        ItemService itemService = new(itemRepository.Object);

        //Act
        //Assert
        await itemService.Invoking(x=> x.Update(id, new ItemAddDto { Name = name, Price = price }))
                            .Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async Task Update_Invalid_NotFoundException()
    {
        Guid id = new Guid();
        string name = "name";
        decimal price = 5.98m;

        //Arrange
        var itemRepository = new Mock<IItemRepository>();
        itemRepository.Setup(m => m.Update(It.IsAny<ItemEntity>()))
                             .ReturnsAsync(2);

        itemRepository.Setup(m => m.Get(id))
                             .ReturnsAsync(new ItemEntity { Id = id });

        ItemService itemService = new(itemRepository.Object);

        //Act
        //Assert
        await itemService.Invoking(x => x.Update(id, new ItemAddDto { Name = name, Price = price }))
                            .Should().ThrowAsync<InvalidOperationException>();
    }

    public async Task Delete(Guid id)
    {
        //await Assert.NotSame<NotFoundException>(async () => await itemService.Update(id, new ItemAddDto { Name = "aa", Price = 5 }));
    }
    public async Task Buy(Guid id, int quantity)
    { 
    }
}
