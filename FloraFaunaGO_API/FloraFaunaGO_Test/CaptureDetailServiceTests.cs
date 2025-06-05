using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FloraFaunaGO_Test;

[TestClass]
public class CaptureDetailServiceTests
{
    private Mock<ICaptureDetailRepository<CaptureDetailsEntities>> _mockRepo;
    private CaptureDetailService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<ICaptureDetailRepository<CaptureDetailsEntities>>();
        _service = new CaptureDetailService(_mockRepo.Object);
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
    public async Task GetAllCaptureDetail_Should_Return_Pagination()
    {
        var pagination = new Pagination<CaptureDetailsEntities>
        {
            Items = new List<CaptureDetailsEntities> { new CaptureDetailsEntities { Id = "1", Shiny = true } }
        };
        _mockRepo.Setup(r => r.GetAllCaptureDetail(CaptureDetailOrderingCriteria.None, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetAllCaptureDetail();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetById_Should_Return_Dto_When_Entity_Exists()
    {
        var entity = new CaptureDetailsEntities { Id = "1", Shiny = true };
        _mockRepo.Setup(r => r.GetById("1")).ReturnsAsync(entity);

        var result = await _service.GetById("1");
        Assert.IsNotNull(result);
        Assert.AreEqual("1", result.CaptureDetail.Id);
    }

    [TestMethod]
    public async Task GetById_Should_Return_Null_When_Entity_Not_Exists()
    {
        _mockRepo.Setup(r => r.GetById("2")).ReturnsAsync((CaptureDetailsEntities?)null);

        var result = await _service.GetById("2");
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetCaptureDetailByCapture_Should_Return_Pagination()
    {
        var pagination = new Pagination<CaptureDetailsEntities>
        {
            Items = new List<CaptureDetailsEntities> { new CaptureDetailsEntities { Id = "1", CaptureId = "cap1" } }
        };
        _mockRepo.Setup(r => r.GetCaptureDetailByCapture("cap1", CaptureDetailOrderingCriteria.ByCapture, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetCaptureDetailByCapture("cap1");
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetCaptureDetailByDate_Should_Return_Pagination()
    {
        var pagination = new Pagination<CaptureDetailsEntities>
        {
            Items = new List<CaptureDetailsEntities> { new CaptureDetailsEntities { Id = "1" } }
        };
        _mockRepo.Setup(r => r.GetCaptureDetailByDate(CaptureDetailOrderingCriteria.ByCaptureDate, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetCaptureDetailByDate();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetCaptureDetailByLocation_Should_Return_Pagination()
    {
        var pagination = new Pagination<CaptureDetailsEntities>
        {
            Items = new List<CaptureDetailsEntities> { new CaptureDetailsEntities { Id = "1" } }
        };
        _mockRepo.Setup(r => r.GetCaptureDetailByLocation(CaptureDetailOrderingCriteria.ByCaptureLocation, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetCaptureDetailByLocation();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task Insert_Should_Call_Repository_And_Return_Dto()
    {
        var dto = new CaptureDetailNormalDto { Shiny = true };
        var entity = new CaptureDetailsEntities { Id = "1", Shiny = true };
        _mockRepo.Setup(r => r.Insert(It.IsAny<CaptureDetailsEntities>())).ReturnsAsync(entity);

        var result = await _service.Insert(dto);
        Assert.IsNotNull(result);
        Assert.AreEqual("1", result.CaptureDetail.Id);
    }

    [TestMethod]
    public async Task Update_Should_Call_Repository_And_Return_Dto()
    {
        var dto = new CaptureDetailNormalDto { Id = "1", Shiny = false };
        var entity = new CaptureDetailsEntities { Id = "1", Shiny = false };
        _mockRepo.Setup(r => r.Update("1", It.IsAny<CaptureDetailsEntities>())).ReturnsAsync(entity);

        var result = await _service.Update("1", dto);
        Assert.IsNotNull(result);
        Assert.AreEqual("1", result.CaptureDetail.Id);
    }
}
