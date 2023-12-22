using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using ShopV2.Exceptions;
using ShopV2.Interfaces;
using ShopV2.Objects.DTO;
using ShopV2.Objects.Entities;
using ShopV2.Services;

namespace ShopV2UnitTest.Services;

public class ShopV2ServiceTests
{

    private readonly Mock<IItemRepository> _itemRepositoryMock;
    private readonly ItemService _itemService;

    public ShopV2ServiceTests()
    {
        _itemRepositoryMock = new Mock<IItemRepository>();
        _itemService = new ItemService(_itemRepositoryMock.Object);
    }

    // naudot verify
    // naudot autodata is autofixture
    [Theory]
    [AutoData]
    public async Task Getid_GivenValidId_ReturnsDTO(Guid id)
    {
        //Arrange
        _itemRepositoryMock.Setup(m => m.Get(id))
                        .ReturnsAsync(new ItemEntity { Id = id });

        //Act
        ItemDto result = await _itemService.Get(id);

        //Assert
        result.Id.Should().Be(id);
    }

    [Fact]
    public async Task Getid_GivenInvalidId_ThrowNotFoundException()
    {
        // Arrange
        Guid id = new();

        _itemRepositoryMock.Setup(m => m.Get(id))
                        .Returns(Task.FromResult<ItemEntity?>(null));

        // Act Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await _itemService.Get(id));
    }

    [Fact]
    public async Task Get_GivenValidId_ReturnsDTO()
    {
        //Arrange
        _itemRepositoryMock.Setup(m => m.Get())
                        .ReturnsAsync(new List<ItemEntity>
                        {
                            new ItemEntity {Id = new Guid()},
                            new ItemEntity {Id = new Guid()}
                        });

        //Act
        var result = await _itemService.Get();

        //Assert
        result.Count.Should().Be(2);
    }

    [Fact]
    public async Task Get_GivenNull_ThrowNotFoundException()
    {
        // Arrange
        _itemRepositoryMock.Setup(m => m.Get())
                        .Returns(Task.FromResult<IEnumerable<ItemEntity>>(null!));

        // Act Assert
        await _itemService.Invoking(x => x.Get())
                    .Should().ThrowAsync<NotFoundException>();

        //        await Assert.ThrowsAsync<NotFoundException>(async () => await itemService.Get());
    }

    [Theory]
    [AutoData]
    public async Task Add_GivenValidId_ReturnsGuid(Guid id, string name, decimal price)
    {
        //Arrange
        _itemRepositoryMock.Setup(m => m.Add(It.Is<ItemEntity>
                                (x => x.Name == name && x.Price == price && x.IsDeleted == false)))
                                 .ReturnsAsync(id);

        //Act
        Guid result = await _itemService.Add(new ItemAddDto { Name = name, Price = price });

        //Assert
        result.Should().Be(id);
    }

    [Theory]
    [AutoData]
    public async Task Update_ReturnsSucces(Guid id, string name, decimal price)
    {
        //Arrange
        _itemRepositoryMock.Setup(m => m.Update(It.Is<ItemEntity>
                                (x => x.Id == id && x.Name == name && x.Price == price)))
                                .ReturnsAsync(1);

        _itemRepositoryMock.Setup(m => m.Get(id))
                                .ReturnsAsync(new ItemEntity
                                { Id = id, Name = name, Price = price, IsDeleted = false });

        //Act
        //Assert
        await _itemService.Invoking(x => x.Update(id, new ItemAddDto
        { Name = name, Price = price }))
                                        .Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async Task Update_Invalid_InvalidOperationException()
    {
        Guid id = new Guid();
        string name = "name";
        decimal price = 5.98m;

        //Arrange
        _itemRepositoryMock.Setup(m => m.Update(It.Is<ItemEntity>
                                (x => x.Id == id && x.Name == name && x.Price == price)))
                                .ReturnsAsync(2);

        _itemRepositoryMock.Setup(m => m.Get(id))
                             .ReturnsAsync(new ItemEntity { Id = id });

        //Act
        //Assert
        await _itemService.Invoking(x => x.Update(id, new ItemAddDto { Name = name, Price = price }))
                            .Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Update_InvalidId_InvalidOperationException()
    {
        Guid id = new Guid();
        string name = "name";
        decimal price = 5.98m;

        //Arrange
        _itemRepositoryMock.Setup(m => m.Update(It.Is<ItemEntity>
                                (x => x.Id == id && x.Name == name && x.Price == price)))
                                .ReturnsAsync(1);

        _itemRepositoryMock.Setup(m => m.Get(id))
                        .Returns(Task.FromResult<ItemEntity?>(null));

        //Act
        //Assert
        await _itemService.Invoking(x => x.Update(id, new ItemAddDto { Name = name, Price = price }))
                            .Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Delete_ValidId()
    {
        Guid id = new Guid();

        //Arrange
        _itemRepositoryMock.Setup(m => m.Delete(id));

        _itemRepositoryMock.Setup(m => m.Get(id))
                        .Returns(Task.FromResult(new ItemEntity { Id = id })!);

        //Act
        //Assert
        await _itemService.Invoking(x => x.Delete(id))
                            .Should().NotThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Delete_InvalidId_ThrowNotFoundException()
    {
        Guid id = new Guid();

        //Arrange
        _itemRepositoryMock.Setup(m => m.Delete(id));

        _itemRepositoryMock.Setup(m => m.Get(id))
                        .Returns(Task.FromResult<ItemEntity?>(null));

        //Act
        //Assert
        await _itemService.Invoking(x => x.Delete(id))
                            .Should().ThrowAsync<NotFoundException>();
    }

    public async Task Buy(Guid id, int quantity)
    {
    }
}
