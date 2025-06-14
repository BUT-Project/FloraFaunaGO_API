using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Moq;

namespace FloraFaunaGO_Test;

[TestClass]
public class SuccessStateTest
{
    private Mock<ISuccessStateRepository<SuccesStateEntities>> _mockRepo;
    private SuccessStateService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<ISuccessStateRepository<SuccesStateEntities>>();
        _service = new SuccessStateService(_mockRepo.Object);
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
    public async Task GetAllSuccessState_Should_Return_Pagination()
    {
        var pagination = new Pagination<SuccesStateEntities>
        {
            Items = new List<SuccesStateEntities> { new SuccesStateEntities { Id = "1", PercentSucces = 50, IsSucces = true } }
        };
        _mockRepo.Setup(r => r.GetAllSuccessState(SuccessStateOrderingCreteria.None, 0, 10)).ReturnsAsync(pagination);

        var result = await _service.GetAllSuccessState();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetById_Should_Return_Dto_When_Entity_Exists()
    {
        var entity = new SuccesStateEntities { Id = "1", PercentSucces = 80, IsSucces = true };
        _mockRepo.Setup(r => r.GetById("1")).ReturnsAsync(entity);

        var result = await _service.GetById("1");
        Assert.IsNotNull(result);
        Assert.AreEqual(80, result.State.PercentSucces);
    }

    [TestMethod]
    public async Task GetById_Should_Return_Null_When_Entity_Not_Exists()
    {
        _mockRepo.Setup(r => r.GetById("2")).ReturnsAsync((SuccesStateEntities?)null);

        var result = await _service.GetById("2");
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetSuccessStateBySuccess_Should_Return_Pagination()
    {
        var pagination = new Pagination<SuccesStateEntities>
        {
            Items = new List<SuccesStateEntities> { new SuccesStateEntities { Id = "1", PercentSucces = 100, IsSucces = true } }
        };
        _mockRepo.Setup(r => r.GetSuccessStateBySuccess(SuccessStateOrderingCreteria.BySuccess, 0, 10)).ReturnsAsync(pagination);

        var result = await _service.GetSuccessStateBySuccess();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetSuccessStateByUser_Should_Return_Pagination()
    {
        var pagination = new Pagination<SuccesStateEntities>
        {
            Items = new List<SuccesStateEntities> { new SuccesStateEntities { Id = "1", PercentSucces = 60, IsSucces = false } }
        };
        _mockRepo.Setup(r => r.GetSuccessStateByUser("user1", SuccessStateOrderingCreteria.ByUser, 0, 10)).ReturnsAsync(pagination);

        var result = await _service.GetSuccessStateByUser("user1");
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task Insert_Should_Call_Repository_And_Return_Dto()
    {
        var dto = new SuccessStateNormalDto { PercentSucces = 90, IsSucces = true };
        var entity = new SuccesStateEntities { Id = "1", PercentSucces = 90, IsSucces = true };
        _mockRepo.Setup(r => r.Insert(It.IsAny<SuccesStateEntities>())).ReturnsAsync(entity);

        var result = await _service.Insert(dto);
        Assert.IsNotNull(result);
        Assert.AreEqual(90, result.State.PercentSucces);
    }

    [TestMethod]
    public async Task Update_Should_Call_Repository_And_Return_Dto()
    {
        var dto = new SuccessStateNormalDto { Id = "1", PercentSucces = 70, IsSucces = false };
        var entity = new SuccesStateEntities { Id = "1", PercentSucces = 70, IsSucces = false };
        _mockRepo.Setup(r => r.Update("1", It.IsAny<SuccesStateEntities>())).ReturnsAsync(entity);

        var result = await _service.Update("1", dto);
        Assert.IsNotNull(result);
        Assert.AreEqual(70, result.State.PercentSucces);
    }

    [TestMethod]
    public async Task GetSuccessStateByUser_Success_Should_Return_Pagination()
    {
        var pagination = new Pagination<SuccesStateEntities>
        {
            Items = new List<SuccesStateEntities> { new SuccesStateEntities { Id = "1", PercentSucces = 100, IsSucces = true } }
        };
        _mockRepo.Setup(r => r.GetSuccessStateByUser_Success("success1", "user1", SuccessStateOrderingCreteria.ByUser, 0, 10)).ReturnsAsync(pagination);

        var result = await _service.GetSuccessStateByUser_Success("success1", "user1");
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }
}
