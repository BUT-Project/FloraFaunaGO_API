using FloraFauna_GO_Dto.Full;
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
public class EspeceTests
{
    private Mock<IEspeceRepository<EspeceEntities>> _mockRepo;
    private EspeceService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IEspeceRepository<EspeceEntities>>();
        _service = new EspeceService(_mockRepo.Object);
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
    public async Task GetAllEspece_Should_Return_Pagination()
    {
        var pagination = new Pagination<EspeceEntities>
        {
            Items = new List<EspeceEntities> { new EspeceEntities { Id = "1", Nom = "Lion" } }
        };
        _mockRepo.Setup(r => r.GetAllEspece(EspeceOrderingCriteria.None, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetAllEspece();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetById_Should_Return_Dto_When_Entity_Exists()
    {
        var entity = new EspeceEntities { Id = "1", Nom = "Lion" };
        _mockRepo.Setup(r => r.GetById("1")).ReturnsAsync(entity);

        var result = await _service.GetById("1");
        Assert.IsNotNull(result);
        Assert.AreEqual("Lion", result.Nom);
    }

    [TestMethod]
    public async Task GetById_Should_Return_Null_When_Entity_Not_Exists()
    {
        _mockRepo.Setup(r => r.GetById("2")).ReturnsAsync((EspeceEntities?)null);

        var result = await _service.GetById("2");
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetEspeceByFamile_Should_Return_Pagination()
    {
        var pagination = new Pagination<EspeceEntities>
        {
            Items = new List<EspeceEntities> { new EspeceEntities { Id = "1", Nom = "Lion", Famille = "Felidae" } }
        };
        _mockRepo.Setup(r => r.GetEspeceByFamile(EspeceOrderingCriteria.ByFamille, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetEspeceByFamile();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetEspeceByName_Should_Return_Pagination()
    {
        var pagination = new Pagination<EspeceEntities>
        {
            Items = new List<EspeceEntities> { new EspeceEntities { Id = "1", Nom = "Lion" } }
        };
        _mockRepo.Setup(r => r.GetEspeceByName("Lion", EspeceOrderingCriteria.ByNom, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetEspeceByName("Lion");
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetEspeceByRegime_Should_Return_Pagination()
    {
        var pagination = new Pagination<EspeceEntities>
        {
            Items = new List<EspeceEntities> { new EspeceEntities { Id = "1", Nom = "Lion", Regime = "Carnivore" } }
        };
        _mockRepo.Setup(r => r.GetEspeceByRegime(EspeceOrderingCriteria.ByRegime, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetEspeceByRegime();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task Insert_Should_Call_Repository_And_Return_Dto()
    {
        var dto = new FullEspeceDto { Nom = "Lion" };
        var entity = new EspeceEntities { Id = "1", Nom = "Lion" };
        _mockRepo.Setup(r => r.Insert(It.IsAny<EspeceEntities>())).ReturnsAsync(entity);

        var result = await _service.Insert(dto);
        Assert.IsNotNull(result);
        Assert.AreEqual("Lion", result.Nom);
    }

    [TestMethod]
    public async Task Update_Should_Call_Repository_And_Return_Dto()
    {
        var dto = new FullEspeceDto { Id = "1", Nom = "Lion" };
        var entity = new EspeceEntities { Id = "1", Nom = "Lion" };
        _mockRepo.Setup(r => r.Update("1", It.IsAny<EspeceEntities>())).ReturnsAsync(entity);

        var result = await _service.Update("1", dto);
        Assert.IsNotNull(result);
        Assert.AreEqual("Lion", result.Nom);
    }

    [TestMethod]
    public async Task GetEspeceByProperty_Should_Return_Pagination()
    {
        var pagination = new Pagination<EspeceEntities>
        {
            Items = new List<EspeceEntities> { new EspeceEntities { Id = "1", Nom = "Lion" } }
        };
        _mockRepo.Setup(r => r.GetEspeceByProperty("1", "Lion", EspeceOrderingCriteria.ByNom, 0, 15)).ReturnsAsync(pagination);

        var result = await _service.GetEspeceByProperty("1", "Lion", EspeceOrderingCriteria.ByNom);
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }
}
