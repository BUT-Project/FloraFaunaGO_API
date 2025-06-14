using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Moq;

namespace FloraFaunaGO_Test;

[TestClass]
public class SuccessServiceTests
{
    private Mock<ISuccessRepository<SuccesEntities>> _mockRepo;
    private SuccessService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<ISuccessRepository<SuccesEntities>>();
        _service = new SuccessService(_mockRepo.Object);
    }

    [TestMethod]
    public async Task Delete_Should_Call_Repository_And_Return_Result()
    {
        _mockRepo.Setup(r => r.Delete("1")).ReturnsAsync(true);
        var result = await _service.Delete("1");
        Assert.IsTrue(result);
        _mockRepo.Verify(r => r.Delete("1"), Times.Once);
    }

    [TestMethod]
    public async Task GetAllSuccess_Should_Return_Pagination()
    {
        var pagination = new Pagination<SuccesEntities>
        {
            Items = new List<SuccesEntities> { new SuccesEntities { Id = "1", Nom = "Test", Type = "Type", Description = "Desc", Objectif = 1, Evenenement = "Ev" } }
        };
        _mockRepo.Setup(r => r.GetAllSuccess(SuccessOrderingCreteria.None, 0, 10)).ReturnsAsync(pagination);

        var result = await _service.GetAllSuccess();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetById_Should_Return_Dto_When_Entity_Exists()
    {
        var entity = new SuccesEntities { Id = "1", Nom = "Test", Type = "Type", Description = "Desc", Objectif = 1, Evenenement = "Ev" };
        _mockRepo.Setup(r => r.GetById("1")).ReturnsAsync(entity);

        var result = await _service.GetById("1");
        Assert.IsNotNull(result);
        Assert.AreEqual("Test", result.Nom);
    }

    [TestMethod]
    public async Task GetById_Should_Return_Null_When_Entity_Not_Exists()
    {
        _mockRepo.Setup(r => r.GetById("2")).ReturnsAsync((SuccesEntities?)null);

        var result = await _service.GetById("2");
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetSuccessByName_Should_Return_Pagination()
    {
        var pagination = new Pagination<SuccesEntities>
        {
            Items = new List<SuccesEntities> { new SuccesEntities { Id = "1", Nom = "Test", Type = "Type", Description = "Desc", Objectif = 1, Evenenement = "Ev" } }
        };
        _mockRepo.Setup(r => r.GetSuccessByName("Test", SuccessOrderingCreteria.ByName, 0, 10)).ReturnsAsync(pagination);

        var result = await _service.GetSuccessByName("Test");
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task Insert_Should_Call_Repository_And_Return_Dto()
    {
        var dto = new SuccessNormalDto { Nom = "Test", Type = "Type", Description = "Desc", Objectif = 1, Evenement = "Ev" };
        var entity = new SuccesEntities { Id = "1", Nom = "Test", Type = "Type", Description = "Desc", Objectif = 1, Evenenement = "Ev" };
        _mockRepo.Setup(r => r.Insert(It.IsAny<SuccesEntities>())).ReturnsAsync(entity);

        var result = await _service.Insert(dto);
        Assert.IsNotNull(result);
        Assert.AreEqual("Test", result.Nom);
    }

    [TestMethod]
    public async Task Update_Should_Call_Repository_And_Return_Dto()
    {
        var dto = new SuccessNormalDto { Id = "1", Nom = "Test", Type = "Type", Description = "Desc", Objectif = 1, Evenement = "Ev" };
        var entity = new SuccesEntities { Id = "1", Nom = "Test", Type = "Type", Description = "Desc", Objectif = 1, Evenenement = "Ev" };
        _mockRepo.Setup(r => r.Update("1", It.IsAny<SuccesEntities>())).ReturnsAsync(entity);

        var result = await _service.Update("1", dto);
        Assert.IsNotNull(result);
        Assert.AreEqual("Test", result.Nom);
    }

    [TestMethod]
    public async Task GetSuccessBySuccessState_Should_Return_Pagination()
    {
        var pagination = new Pagination<SuccesEntities>
        {
            Items = new List<SuccesEntities> { new SuccesEntities { Id = "1", Nom = "Test", Type = "Type", Description = "Desc", Objectif = 1, Evenenement = "Ev" } }
        };
        _mockRepo.Setup(r => r.GetSuccessBySuccessState("1", SuccessOrderingCreteria.None, 0, 10)).ReturnsAsync(pagination);

        var result = await _service.GetSuccessBySuccessState("1");
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }
}