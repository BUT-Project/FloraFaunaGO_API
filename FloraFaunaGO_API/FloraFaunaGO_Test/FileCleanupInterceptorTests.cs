using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Shared.Interfaces;
using FloraFaunaGO_Services;

namespace FloraFaunaGO_Test;

[TestClass]
public class FileCleanupInterceptorTests
{
    private Mock<IFileStorageService> _mockFileStorageService;
    private Mock<ILogger<FileCleanupInterceptor>> _mockLogger;
    private FileCleanupInterceptor _interceptor;
    private FloraFaunaGoDB _context;

    [TestInitialize]
    public void Setup()
    {
        _mockFileStorageService = new Mock<IFileStorageService>();
        _mockLogger = new Mock<ILogger<FileCleanupInterceptor>>();
        
        _interceptor = new FileCleanupInterceptor(_mockFileStorageService.Object, _mockLogger.Object);

        var options = new DbContextOptionsBuilder<FloraFaunaGoDB>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .AddInterceptors(_interceptor)
            .Options;

        _context = new FloraFaunaGoDB(options);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }

    [TestMethod]
    public async Task UserPhotoUpdate_DeletesOldFile_WhenImageUrlChanges()
    {
        // Arrange
        var user = new UtilisateurEntities
        {
            Id = "user1",
            UserName = "TestUser",
            ImageUrl = "images/old-photo.jpg",
            Email = "test@test.com"
        };

        _context.Utilisateur.Add(user);
        await _context.SaveChangesAsync();

        _mockFileStorageService.Setup(x => x.DeleteAsync("images/old-photo.jpg"))
            .ReturnsAsync(true);

        // Act
        user.ImageUrl = "images/new-photo.jpg";
        await _context.SaveChangesAsync();

        // Assert
        _mockFileStorageService.Verify(x => x.DeleteAsync("images/old-photo.jpg"), Times.Once);
        _mockFileStorageService.Verify(x => x.DeleteAsync("images/new-photo.jpg"), Times.Never);
    }

    [TestMethod]
    public async Task UserPhotoUpdate_DoesNotDeleteFile_WhenImageUrlUnchanged()
    {
        // Arrange
        var user = new UtilisateurEntities
        {
            Id = "user1",
            UserName = "TestUser",
            ImageUrl = "images/same-photo.jpg",
            Email = "test@test.com"
        };

        _context.Utilisateur.Add(user);
        await _context.SaveChangesAsync();

        // Act - Update different property
        user.UserName = "UpdatedUser";
        await _context.SaveChangesAsync();

        // Assert
        _mockFileStorageService.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task UserPhotoUpdate_DoesNotDeleteFile_WhenOriginalImageUrlIsNull()
    {
        // Arrange
        var user = new UtilisateurEntities
        {
            Id = "user1",
            UserName = "TestUser",
            ImageUrl = null,
            Email = "test@test.com"
        };

        _context.Utilisateur.Add(user);
        await _context.SaveChangesAsync();

        // Act
        user.ImageUrl = "images/new-photo.jpg";
        await _context.SaveChangesAsync();

        // Assert
        _mockFileStorageService.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task EspeceImageUpdate_DeletesOldFiles_WhenBothImagesChange()
    {
        // Arrange
        var espece = new EspeceEntities
        {
            Id = "espece1",
            Nom = "TestSpecies",
            ImageUrl = "images/old-species.jpg",
            Image3DUrl = "images/old-species-3d.obj",
            Description = "Test description",
            Famille = "TestFamily"
        };

        _context.Espece.Add(espece);
        await _context.SaveChangesAsync();

        _mockFileStorageService.Setup(x => x.DeleteAsync("images/old-species.jpg"))
            .ReturnsAsync(true);
        _mockFileStorageService.Setup(x => x.DeleteAsync("images/old-species-3d.obj"))
            .ReturnsAsync(true);

        // Act
        espece.ImageUrl = "images/new-species.jpg";
        espece.Image3DUrl = "images/new-species-3d.obj";
        await _context.SaveChangesAsync();

        // Assert
        _mockFileStorageService.Verify(x => x.DeleteAsync("images/old-species.jpg"), Times.Once);
        _mockFileStorageService.Verify(x => x.DeleteAsync("images/old-species-3d.obj"), Times.Once);
    }

    [TestMethod]
    public async Task CapturePhotoUpdate_DeletesOldFile_WhenPhotoUrlChanges()
    {
        // Arrange
        var capture = new CaptureEntities
        {
            Id = "capture1",
            PhotoUrl = "images/old-capture.jpg",
            EspeceId = "espece1",
            UtilisateurId = "user1"
        };

        _context.Captures.Add(capture);
        await _context.SaveChangesAsync();

        _mockFileStorageService.Setup(x => x.DeleteAsync("images/old-capture.jpg"))
            .ReturnsAsync(true);

        // Act
        capture.PhotoUrl = "images/new-capture.jpg";
        await _context.SaveChangesAsync();

        // Assert
        _mockFileStorageService.Verify(x => x.DeleteAsync("images/old-capture.jpg"), Times.Once);
    }

    [TestMethod]
    public async Task UserDeletion_DeletesAssociatedFile()
    {
        // Arrange
        var user = new UtilisateurEntities
        {
            Id = "user1",
            UserName = "TestUser",
            ImageUrl = "images/user-photo.jpg",
            Email = "test@test.com"
        };

        _context.Utilisateur.Add(user);
        await _context.SaveChangesAsync();

        _mockFileStorageService.Setup(x => x.DeleteAsync("images/user-photo.jpg"))
            .ReturnsAsync(true);

        // Act
        _context.Utilisateur.Remove(user);
        await _context.SaveChangesAsync();

        // Assert
        _mockFileStorageService.Verify(x => x.DeleteAsync("images/user-photo.jpg"), Times.Once);
    }

    [TestMethod]
    public async Task FileCleanup_HandlesFileStorageException_Gracefully()
    {
        // Arrange
        var user = new UtilisateurEntities
        {
            Id = "user1",
            UserName = "TestUser",
            ImageUrl = "images/old-photo.jpg",
            Email = "test@test.com"
        };

        _context.Utilisateur.Add(user);
        await _context.SaveChangesAsync();

        _mockFileStorageService.Setup(x => x.DeleteAsync("images/old-photo.jpg"))
            .ThrowsAsync(new Exception("Storage service error"));

        // Act & Assert - Should not throw exception
        user.ImageUrl = "images/new-photo.jpg";
        await _context.SaveChangesAsync();

        // Verify error was logged
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error deleting file")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [TestMethod]
    public async Task MultipleEntityUpdates_DeletesCorrectFiles()
    {
        // Arrange
        var user = new UtilisateurEntities
        {
            Id = "user1",
            UserName = "TestUser",
            ImageUrl = "images/old-user.jpg",
            Email = "test@test.com"
        };

        var espece = new EspeceEntities
        {
            Id = "espece1",
            Nom = "TestSpecies",
            ImageUrl = "images/old-species.jpg",
            Description = "Test description",
            Famille = "TestFamily"
        };

        _context.Utilisateur.Add(user);
        _context.Espece.Add(espece);
        await _context.SaveChangesAsync();

        _mockFileStorageService.Setup(x => x.DeleteAsync("images/old-user.jpg"))
            .ReturnsAsync(true);
        _mockFileStorageService.Setup(x => x.DeleteAsync("images/old-species.jpg"))
            .ReturnsAsync(true);

        // Act
        user.ImageUrl = "images/new-user.jpg";
        espece.ImageUrl = "images/new-species.jpg";
        await _context.SaveChangesAsync();

        // Assert
        _mockFileStorageService.Verify(x => x.DeleteAsync("images/old-user.jpg"), Times.Once);
        _mockFileStorageService.Verify(x => x.DeleteAsync("images/old-species.jpg"), Times.Once);
    }
}