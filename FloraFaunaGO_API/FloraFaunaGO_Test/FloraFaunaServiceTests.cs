using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using Moq;

namespace FloraFaunaGO_Test;

[TestClass]
public class FloraFaunaServiceTests
{
    private Mock<IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities, UtilisateurEntities, SuccesEntities, SuccesStateEntities, LocalisationEntities>> _mockDbUow;
    private FloraFaunaService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockDbUow = new Mock<IUnitOfWork<EspeceEntities, CaptureEntities, CaptureDetailsEntities, UtilisateurEntities, SuccesEntities, SuccesStateEntities, LocalisationEntities>>();
        _service = new FloraFaunaService(_mockDbUow.Object);
    }

    [TestMethod]
    public async Task SaveChangesAsync_Should_Transform_Entities_To_Dto()
    {
        var entities = new List<object?>
        {
            new EspeceEntities { Id = "esp1", Nom = "Lion" },
            new UtilisateurEntities { Id = "u1", UserName = "Test" },
            new SuccesEntities { Id = "s1", Nom = "Succès" },
            new CaptureEntities { Id = "c1", EspeceId = "esp1" },
            new CaptureDetailsEntities { Id = "cd1", Shiny = true },
            new LocalisationEntities { Id = "l1", Latitude = 1.0 },
            new SuccesStateEntities { Id = "ss1", PercentSucces = 100 }
        };
        _mockDbUow.Setup(u => u.SaveChangesAsync()).ReturnsAsync(entities);

        var result = await _service.SaveChangesAsync();

        Assert.IsNotNull(result);
        Assert.AreEqual(7, result.Count());
    }

    [TestMethod]
    public async Task SaveChangesAsync_Should_Return_Null_If_Uow_Returns_Null()
    {
        _mockDbUow.Setup(u => u.SaveChangesAsync()).ReturnsAsync((IEnumerable<object?>?)null);
        var result = await _service.SaveChangesAsync();
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task AddSuccesStateAsync_Should_Call_Uow()
    {
        var successState = new SuccessStateNormalDto { Id = "ss1" };
        var user = new UtilisateurNormalDto { Id = "u1" };
        var success = new SuccessNormalDto { Id = "s1" };
        _mockDbUow.Setup(u => u.AddSuccesStateAsync(It.IsAny<SuccesStateEntities>(), It.IsAny<UtilisateurEntities>(), It.IsAny<SuccesEntities>())).ReturnsAsync(true);

        var result = await _service.AddSuccesStateAsync(successState, user, success);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task DeleteSuccesStateAsync_Should_Call_Uow()
    {
        var successState = new SuccessStateNormalDto { Id = "ss1" };
        var user = new UtilisateurNormalDto { Id = "u1" };
        var success = new SuccessNormalDto { Id = "s1" };
        _mockDbUow.Setup(u => u.DeleteSuccesStateAsync(It.IsAny<SuccesStateEntities>(), It.IsAny<UtilisateurEntities>(), It.IsAny<SuccesEntities>())).ReturnsAsync(true);

        var result = await _service.DeleteSuccesStateAsync(successState, user, success);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task AddCaptureAsync_Should_Call_Uow()
    {
        var capture = new CaptureNormalDto { Id = "c1", IdEspece = "esp1" };
        var user = new UtilisateurNormalDto { Id = "u1" };
        _mockDbUow.Setup(u => u.AddCaptureAsync(It.IsAny<CaptureEntities>(), It.IsAny<UtilisateurEntities>())).ReturnsAsync(true);

        var result = await _service.AddCaptureAsync(capture, user);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task DeleteCaptureAsync_Should_Call_Uow()
    {
        var capture = new CaptureNormalDto { Id = "c1", IdEspece = "esp1" };
        var user = new UtilisateurNormalDto { Id = "u1" };
        var details = new List<CaptureDetailNormalDto> { new CaptureDetailNormalDto { Id = "cd1" } };
        _mockDbUow.Setup(u => u.DeleteCaptureAsync(It.IsAny<CaptureEntities>(), It.IsAny<UtilisateurEntities>(), It.IsAny<IEnumerable<CaptureDetailsEntities>>())).ReturnsAsync(true);

        var result = await _service.DeleteCaptureAsync(capture, user, details);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task AddCaptureDetailAsync_Should_Call_Uow()
    {
        var detail = new CaptureDetailNormalDto { Id = "cd1" };
        var capture = new CaptureNormalDto { Id = "c1" };
        var localisation = new LocalisationNormalDto { Id = "l1" };
        _mockDbUow.Setup(u => u.AddCaptureDetailAsync(It.IsAny<CaptureDetailsEntities>(), It.IsAny<CaptureEntities>(), It.IsAny<LocalisationEntities>())).ReturnsAsync(true);

        var result = await _service.AddCaptureDetailAsync(detail, capture, localisation);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task DeleteCaptureDetailAsync_Should_Call_Uow()
    {
        var detail = new CaptureDetailNormalDto { Id = "cd1" };
        var capture = new CaptureNormalDto { Id = "c1" };
        var localisation = new LocalisationNormalDto { Id = "l1" };
        _mockDbUow.Setup(u => u.DeleteCaptureDetailAsync(It.IsAny<CaptureDetailsEntities>(), It.IsAny<CaptureEntities>(), It.IsAny<LocalisationEntities>())).ReturnsAsync(true);

        var result = await _service.DeleteCaptureDetailAsync(detail, capture, localisation);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task AddEspeceAsync_Should_Call_Uow()
    {
        var espece = new FullEspeceDto { Id = "esp1", Nom = "Lion" };
        var localisations = new List<LocalisationNormalDto> { new LocalisationNormalDto { Id = "l1" } };
        _mockDbUow.Setup(u => u.AddEspeceAsync(It.IsAny<EspeceEntities>(), It.IsAny<IEnumerable<LocalisationEntities>>())).ReturnsAsync(true);

        var result = await _service.AddEspeceAsync(espece, localisations);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task DeleteEspeceAsync_Should_Call_Uow()
    {
        var espece = new FullEspeceDto { Id = "esp1", Nom = "Lion" };
        var localisations = new List<LocalisationNormalDto> { new LocalisationNormalDto { Id = "l1" } };
        _mockDbUow.Setup(u => u.DeleteEspeceAsync(It.IsAny<EspeceEntities>(), It.IsAny<IEnumerable<LocalisationEntities>>())).ReturnsAsync(true);

        var result = await _service.DeleteEspeceAsync(espece, localisations);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task AddSuccess_Should_Call_Uow()
    {
        var success = new SuccessNormalDto { Id = "s1", Nom = "Succès" };
        _mockDbUow.Setup(u => u.AddSuccess(It.IsAny<SuccesEntities>())).ReturnsAsync(true);

        var result = await _service.AddSuccess(success);
        Assert.IsTrue(result);
    }
}
