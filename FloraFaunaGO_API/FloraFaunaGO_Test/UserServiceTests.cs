using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using FloraFauna_GO_Shared;
using FloraFauna_GO_Shared.Criteria;
using Moq;

namespace FloraFaunaGO_Test;

[TestClass]
public class UserServiceTests
{
    private Mock<IUserRepository<UtilisateurEntities>> _mockRepo;
    private UserService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IUserRepository<UtilisateurEntities>>();
        _service = new UserService(_mockRepo.Object);
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
    public async Task GetAllUser_Should_Return_Pagination()
    {
        var pagination = new Pagination<UtilisateurEntities>
        {
            Items = new List<UtilisateurEntities> { new UtilisateurEntities { Id = "1", UserName = "Test" } }
        };
        _mockRepo.Setup(r => r.GetAllUser(UserOrderingCriteria.None, 0, 10)).ReturnsAsync(pagination);

        var result = await _service.GetAllUser();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetById_Should_Return_Dto_When_Entity_Exists()
    {
        var entity = new UtilisateurEntities { Id = "1", UserName = "Test" };
        _mockRepo.Setup(r => r.GetById("1")).ReturnsAsync(entity);

        var result = await _service.GetById("1");
        Assert.IsNotNull(result);
        Assert.AreEqual("1", result.Utilisateur.Id);
    }

    [TestMethod]
    public async Task GetById_Should_Return_Null_When_Entity_Not_Exists()
    {
        _mockRepo.Setup(r => r.GetById("2")).ReturnsAsync((UtilisateurEntities?)null);

        var result = await _service.GetById("2");
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetUserById_Should_Return_Pagination()
    {
        var pagination = new Pagination<UtilisateurEntities>
        {
            Items = new List<UtilisateurEntities> { new UtilisateurEntities { Id = "1" } }
        };
        _mockRepo.Setup(r => r.GetUserById(UserOrderingCriteria.Id, 0, 5)).ReturnsAsync(pagination);

        var result = await _service.GetUserById();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetUserMail_Should_Return_Pagination()
    {
        var pagination = new Pagination<UtilisateurEntities>
        {
            Items = new List<UtilisateurEntities> { new UtilisateurEntities { Id = "1", Email = "test@mail.com" } }
        };
        _mockRepo.Setup(r => r.GetUserMail(UserOrderingCriteria.Mail, 0, 5)).ReturnsAsync(pagination);

        var result = await _service.GetUserMail();
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetUserBySuccessState_Should_Return_Pagination()
    {
        var pagination = new Pagination<UtilisateurEntities>
        {
            Items = new List<UtilisateurEntities> { new UtilisateurEntities { Id = "1" } }
        };
        _mockRepo.Setup(r => r.GetUserBySuccessState("ss1", UserOrderingCriteria.None, 0, 5)).ReturnsAsync(pagination);

        var result = await _service.GetUserBySuccessState("ss1");
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task Insert_Should_Call_Repository_And_Return_Dto()
    {
        var dto = new UtilisateurNormalDto { Pseudo = "Test", Mail = "test@mail.com", Hash_mdp = "hash", DateInscription = System.DateTime.Now };
        var entity = new UtilisateurEntities { Id = "1", UserName = "Test", Email = "test@mail.com" };
        _mockRepo.Setup(r => r.Insert(It.IsAny<UtilisateurEntities>())).ReturnsAsync(entity);

        var result = await _service.Insert(dto);
        Assert.IsNotNull(result);
        Assert.AreEqual("Test", result.Utilisateur.Pseudo);
    }

    [TestMethod]
    public async Task Update_Should_Call_Repository_And_Return_Dto()
    {
        var dto = new UtilisateurNormalDto { Id = "1", Pseudo = "Test" };
        var entity = new UtilisateurEntities { Id = "1", UserName = "Test" };
        _mockRepo.Setup(r => r.Update("1", It.IsAny<UtilisateurEntities>())).ReturnsAsync(entity);

        var result = await _service.Update("1", dto);
        Assert.IsNotNull(result);
        Assert.AreEqual("Test", result.Utilisateur.Pseudo);
    }

    [TestMethod]
    public async Task GetUserByCapture_Should_Return_Pagination()
    {
        var pagination = new Pagination<UtilisateurEntities>
        {
            Items = new List<UtilisateurEntities> { new UtilisateurEntities { Id = "1" } }
        };
        _mockRepo.Setup(r => r.GetUserByCapture("cap1", UserOrderingCriteria.None, 0, 5)).ReturnsAsync(pagination);

        var result = await _service.GetUserByCapture("cap1");
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }

    [TestMethod]
    public async Task GetUserByMail_Should_Return_Pagination()
    {
        var pagination = new Pagination<UtilisateurEntities>
        {
            Items = new List<UtilisateurEntities> { new UtilisateurEntities { Id = "1", Email = "test@mail.com" } }
        };
        _mockRepo.Setup(r => r.GetUserByMail("test@mail.com", UserOrderingCriteria.Mail, 0, 5)).ReturnsAsync(pagination);

        var result = await _service.GetUserByMail("test@mail.com");
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Items.Any());
    }
}
