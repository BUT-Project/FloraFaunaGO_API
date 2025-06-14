using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Moq;

namespace FloraFaunaGO_Test;

[TestClass]
public class CaptureServiceTests
{
    private Mock<ICaptureRepository<CaptureEntities>> _mockRepo;
    private CaptureService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<ICaptureRepository<CaptureEntities>>();
        _service = new CaptureService(_mockRepo.Object);
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
    public async Task GetAllCapture_Should_Return_Pagination()
    {
        var pagination = new Pagination<CaptureEntities>
        {
            Items = new List<CaptureEntities> { new CaptureEntities { Id = "1", EspeceId = "esp1" } }
        };
        _mockRepo.Setup(r => r.GetAllCapture(CaptureOrderingCriteria.None, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetAllCapture();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetById_Should_Return_Dto_When_Entity_Exists()
    {
        var entity = new CaptureEntities { Id = "1", EspeceId = "esp1" };
        _mockRepo.Setup(r => r.GetById("1")).ReturnsAsync(entity);

        var result = await _service.GetById("1");
        Assert.IsNotNull(result);
        Assert.AreEqual("1", result.Capture.Id);
    }

    [TestMethod]
    public async Task GetById_Should_Return_Null_When_Entity_Not_Exists()
    {
        _mockRepo.Setup(r => r.GetById("2")).ReturnsAsync((CaptureEntities?)null);

        var result = await _service.GetById("2");
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetCaptureByNumero_Should_Return_Pagination()
    {
        var pagination = new Pagination<CaptureEntities>
        {
            Items = new List<CaptureEntities> { new CaptureEntities { Id = "1", Numero = 42 } }
        };
        _mockRepo.Setup(r => r.GetCaptureByNumero(CaptureOrderingCriteria.ByNumero, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetCaptureByNumero();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task Insert_Should_Call_Repository_And_Return_Dto()
    {
        var dto = new CaptureNormalDto { IdEspece = "esp1" };
        var entity = new CaptureEntities { Id = "1", EspeceId = "esp1" };
        _mockRepo.Setup(r => r.Insert(It.IsAny<CaptureEntities>())).ReturnsAsync(entity);

        var result = await _service.Insert(dto);
        Assert.IsNotNull(result);
        Assert.AreEqual("1", result.Capture.Id);
    }

    [TestMethod]
    public async Task Update_Should_Call_Repository_And_Return_Dto()
    {
        var dto = new CaptureNormalDto { Id = "1", IdEspece = "esp1" };
        var entity = new CaptureEntities { Id = "1", EspeceId = "esp1" };
        _mockRepo.Setup(r => r.Update("1", It.IsAny<CaptureEntities>())).ReturnsAsync(entity);

        var result = await _service.Update("1", dto);
        Assert.IsNotNull(result);
        Assert.AreEqual("1", result.Capture.Id);
    }

    [TestMethod]
    public async Task GetCaptureByUser_Should_Return_Pagination()
    {
        var pagination = new Pagination<CaptureEntities>
        {
            Items = new List<CaptureEntities> { new CaptureEntities { Id = "1", UtilisateurId = "user1" } }
        };
        _mockRepo.Setup(r => r.GetCaptureByUser("user1", CaptureOrderingCriteria.ByUser, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetCaptureByUser("user1");
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetCaptureByCaptureDetail_Should_Return_Pagination()
    {
        var pagination = new Pagination<CaptureEntities>
        {
            Items = new List<CaptureEntities> { new CaptureEntities { Id = "1" } }
        };
        _mockRepo.Setup(r => r.GetCaptureByCaptureDetail("cd1", CaptureOrderingCriteria.None, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetCaptureByCaptureDetail("cd1");
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetCaptureByEspece_Should_Return_Pagination()
    {
        var pagination = new Pagination<CaptureEntities>
        {
            Items = new List<CaptureEntities> { new CaptureEntities { Id = "1", EspeceId = "esp1" } }
        };
        _mockRepo.Setup(r => r.GetCaptureByEspece("esp1", CaptureOrderingCriteria.None, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetCaptureByEspece("esp1");
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }
}
