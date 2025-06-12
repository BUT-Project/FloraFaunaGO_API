using FloraFauna_GO_Dto.Full;
using FloraFauna_GO_Dto.Normal;
using FloraFauna_GO_Entities;
using FloraFauna_GO_Entities2Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FloraFaunaGO_Test;

[TestClass]
public class ExtensionMapperTests
{
    [TestMethod]
    public void ToEntities_LocalisationNormalDto_To_LocalisationEntities()
    {
        var dto = new LocalisationNormalDto
        {
            Id = "loc1",
            Latitude = 1.1,
            Longitude = 2.2,
            Rayon = 3.3,
            Altitude = 4.4,
            Exactitude = 5.5
        };

        var entity = dto.ToEntities();

        Assert.AreEqual(dto.Latitude, entity.Latitude);
        Assert.AreEqual(dto.Longitude, entity.Longitude);
        Assert.AreEqual(dto.Rayon, entity.Rayon);
        Assert.AreEqual(dto.Altitude, entity.Altitude);
        Assert.AreEqual(dto.Exactitude, entity.Exactitude);
    }

    [TestMethod]
    public void ToEntities_LocalisationNormalDto_WithId()
    {
        var dto = new LocalisationNormalDto { Id = "loc2", Latitude = 10 };
        var entity = dto.ToEntities("loc2");
        Assert.AreEqual("loc2", entity.Id);
        Assert.AreEqual(10, entity.Latitude);
    }

    [TestMethod]
    public void ToDto_LocalisationEntities_To_LocalisationNormalDto()
    {
        var entity = new LocalisationEntities
        {
            Id = "loc3",
            Latitude = 7.7,
            Longitude = 8.8,
            Rayon = 9.9,
            Altitude = 10.1,
            Exactitude = 11.2
        };
        var dto = entity.ToDto();
        Assert.AreEqual(entity.Id, dto.Id);
        Assert.AreEqual(entity.Latitude, dto.Latitude);
        Assert.AreEqual(entity.Longitude, dto.Longitude);
        Assert.AreEqual(entity.Rayon, dto.Rayon);
        Assert.AreEqual(entity.Altitude, dto.Altitude);
        Assert.AreEqual(entity.Exactitude, dto.Exactitude);
    }

    [TestMethod]
    public void ToEntities_CaptureNormalDto_To_CaptureEntities()
    {
        var dto = new CaptureNormalDto
        {
            IdEspece = "esp1",
            photo = new byte[] { 1, 2, 3 },
            Shiny = true,
            LocalisationNormalDto = new LocalisationNormalDto { Latitude = 1 }
        };
        var entity = dto.ToEntities();
        Assert.AreEqual(dto.IdEspece, entity.EspeceId);
        Assert.IsTrue(entity.CaptureDetails.First().Shiny);
        Assert.AreEqual(1, entity.CaptureDetails.First().Localisation.Latitude);
    }

    [TestMethod]
    public void ToEntities_CaptureNormalDto_WithId()
    {
        var dto = new CaptureNormalDto { Id = "cap1", IdEspece = "esp2" };
        var entity = dto.ToEntities("cap1");
        Assert.AreEqual("cap1", entity.Id);
        Assert.AreEqual("esp2", entity.EspeceId);
    }

    [TestMethod]
    public void ToDto_CaptureEntities_To_CaptureNormalDto()
    {
        var entity = new CaptureEntities
        {
            Id = "cap2",
            Photo = new byte[] { 4, 5, 6 }
        };
        var dto = entity.ToDto();
        Assert.AreEqual("cap2", dto.Id);
        CollectionAssert.AreEqual(new byte[] { 4, 5, 6 }, dto.photo);
    }

    [TestMethod]
    public void ToEntities_CaptureDetailNormalDto_To_CaptureDetailsEntities()
    {
        var dto = new CaptureDetailNormalDto { Shiny = true };
        var entity = dto.ToEntities();
        Assert.IsTrue(entity.Shiny);
    }

    [TestMethod]
    public void ToEntities_CaptureDetailNormalDto_WithId()
    {
        var dto = new CaptureDetailNormalDto { Id = "cd1", Shiny = false };
        var entity = dto.ToEntities("cd1");
        Assert.AreEqual("cd1", entity.Id);
        Assert.IsFalse(entity.Shiny);
    }

    [TestMethod]
    public void ToDto_CaptureDetailsEntities_To_CaptureDetailNormalDto()
    {
        var entity = new CaptureDetailsEntities { Id = "cd2", Shiny = true };
        var dto = entity.ToDto();
        Assert.AreEqual("cd2", dto.Id);
        Assert.IsTrue(dto.Shiny);
    }

    [TestMethod]
    public void ToEntities_FullEspeceDto_To_EspeceEntities()
    {
        var dto = new FullEspeceDto
        {
            Nom = "Lion",
            Nom_Scientifique = "Panthera leo",
            Description = "Roi de la savane",
            Image = new byte[] { 1 },
            Image3D = new byte[] { 2 },
            Climat = "Savane",
            Zone = "Afrique",
            Famille = "Felidae",
            Regime = "Carnivore",
            Kingdom = "Animalia",
            Class = "Mammals"
        };
        var entity = dto.ToEntities();
        Assert.AreEqual("Lion", entity.Nom);
        Assert.AreEqual("Panthera leo", entity.Nom_scientifique);
        Assert.AreEqual("Roi de la savane", entity.Description);
        Assert.AreEqual("Felidae", entity.Famille);
        Assert.AreEqual("Carnivore", entity.Regime);
        Assert.AreEqual("Savane", entity.Climat);
        Assert.AreEqual("Afrique", entity.Zone);
        Assert.AreEqual("Animalia", entity.Kingdom);
        Assert.AreEqual("Mammals", entity.Class);
    }

    [TestMethod]
    public void ToEntities_FullEspeceDto_WithId()
    {
        var dto = new FullEspeceDto
        {
            Id = "esp1",
            Nom = "Lion",
            Nom_Scientifique = "Panthera leo",
            Description = "Roi de la savane",
            Image = new byte[] { 1 },
            Image3D = new byte[] { 2 },
            Climat = "Savane",
            Zone = "Afrique",
            Famille = "Felidae",
            Regime = "Carnivore",
            Kingdom = "Animalia",
            Class = "Mammals"
        };
        var entity = dto.ToEntities("esp1");
        Assert.AreEqual("esp1", entity.Id);
        Assert.AreEqual("Lion", entity.Nom);
        Assert.AreEqual("Panthera leo", entity.Nom_scientifique);
        Assert.AreEqual("Roi de la savane", entity.Description);
        Assert.AreEqual("Felidae", entity.Famille);
        Assert.AreEqual("Carnivore", entity.Regime);
        Assert.AreEqual("Savane", entity.Climat);
        Assert.AreEqual("Afrique", entity.Zone);
        Assert.AreEqual("Animalia", entity.Kingdom);
        Assert.AreEqual("Mammals", entity.Class);
    }


    [TestMethod]
    public void ToDto_EspeceEntities_To_FullEspeceDto()
    {
        var entity = new EspeceEntities
        {
            Id = "esp2",
            Nom = "Tigre",
            Nom_scientifique = "Panthera tigris",
            Description = "Grand félin",
            Image = new byte[] { 3 },
            Image3D = new byte[] { 4 },
            Climat = "Forêt",
            Zone = "Asie",
            Famille = "Felidae",
            Regime = "Carnivore",
            Kingdom = "Animalia",
            Class = "Mammals"
        };
        var dto = entity.ToDto();
        Assert.AreEqual("esp2", dto.Id);
        Assert.AreEqual("Tigre", dto.Nom);
        Assert.AreEqual("Panthera tigris", dto.Nom_Scientifique);
        Assert.AreEqual("Grand félin", dto.Description);
        Assert.AreEqual("Felidae", dto.Famille);
        Assert.AreEqual("Carnivore", dto.Regime);
    }

    [TestMethod]
    public void ToEntities_SuccessNormalDto_To_SuccesEntities()
    {
        var dto = new SuccessNormalDto
        {
            Nom = "Succès 1",
            Type = "TypeA",
            Image = "img.png",
            Objectif = 100,
            Description = "Desc",
            Evenement = "Event"
        };
        var entity = dto.ToEntities();
        Assert.AreEqual("Succès 1", entity.Nom);
        Assert.AreEqual("TypeA", entity.Type);
        Assert.AreEqual("img.png", entity.Image);
        Assert.AreEqual(100, entity.Objectif);
        Assert.AreEqual("Desc", entity.Description);
        Assert.AreEqual("Event", entity.Evenenement);
    }

    [TestMethod]
    public void ToEntities_SuccessNormalDto_WithId()
    {
        var dto = new SuccessNormalDto { Id = "s1", Nom = "Succès 2" };
        var entity = dto.ToEntities("s1");
        Assert.AreEqual("s1", entity.Id);
        Assert.AreEqual("Succès 2", entity.Nom);
    }

    [TestMethod]
    public void ToDto_SuccesEntities_To_SuccessNormalDto()
    {
        var entity = new SuccesEntities
        {
            Id = "s2",
            Nom = "Succès 3",
            Type = "TypeB",
            Image = "img2.png",
            Objectif = 200,
            Description = "Desc2",
            Evenenement = "Event2"
        };
        var dto = entity.ToDto();
        Assert.AreEqual("s2", dto.Id);
        Assert.AreEqual("Succès 3", dto.Nom);
        Assert.AreEqual("TypeB", dto.Type);
        Assert.AreEqual("img2.png", dto.Image);
        Assert.AreEqual(200, dto.Objectif);
        Assert.AreEqual("Desc2", dto.Description);
        Assert.AreEqual("Event2", dto.Evenement);
    }

    [TestMethod]
    public void ToEntities_SuccessStateNormalDto_To_SuccesStateEntities()
    {
        var dto = new SuccessStateNormalDto { PercentSucces = 80, IsSucces = true };
        var entity = dto.ToEntities();
        Assert.AreEqual(80, entity.PercentSucces);
        Assert.IsTrue(entity.IsSucces);
    }

    [TestMethod]
    public void ToEntities_SuccessStateNormalDto_WithId()
    {
        var dto = new SuccessStateNormalDto { Id = "ss1", PercentSucces = 90, IsSucces = false };
        var entity = dto.ToEntities("ss1");
        Assert.AreEqual("ss1", entity.Id);
        Assert.AreEqual(90, entity.PercentSucces);
        Assert.IsFalse(entity.IsSucces);
    }

    [TestMethod]
    public void ToDto_SuccesStateEntities_To_SuccessStateNormalDto()
    {
        var entity = new SuccesStateEntities { Id = "ss2", PercentSucces = 100, IsSucces = true };
        var dto = entity.ToDto();
        Assert.AreEqual("ss2", dto.Id);
        Assert.AreEqual(100, dto.PercentSucces);
        Assert.IsTrue(dto.IsSucces);
    }

    [TestMethod]
    public void AddRange_Should_Add_All_Items()
    {
        var set = new HashSet<int> { 1, 2 };
        set.AddRange(new[] { 2, 3, 4 });
        Assert.IsTrue(set.SetEquals(new[] { 1, 2, 3, 4 }));
    }

    [TestMethod]
    public void ToEntities_CaptureDetailNormalDto_WithDate()
    {
        var date = new DateTime(2024, 1, 1);
        var dto = new CaptureDetailNormalDto { Id = "cd3", Shiny = true, date = date };
        var entity = dto.ToEntities("cd3");
        Assert.AreEqual("cd3", entity.Id);
        Assert.AreEqual(date, entity.DateCapture);
        Assert.IsTrue(entity.Shiny);
    }

    [TestMethod]
    public void ToResponseDto_CaptureDetailsEntities()
    {
        var entity = new CaptureDetailsEntities
        {
            Id = "cd4",
            Shiny = true,
            DateCapture = new DateTime(2024, 2, 2),
            LocalisationId = "locX"
        };
        var dto = entity.ToResponseDto();
        Assert.AreEqual("cd4", dto.CaptureDetail.Id);
        Assert.AreEqual(true, dto.CaptureDetail.Shiny);
        Assert.AreEqual(new DateTime(2024, 2, 2), dto.CaptureDetail.date);
        Assert.AreEqual("locX", dto.localisationNormalDtos.Id);
    }

    [TestMethod]
    public void ToEntities_UtilisateurNormalDto()
    {
        var date = DateTime.Now;
        var dto = new UtilisateurNormalDto
        {
            Id = "u1",
            Pseudo = "toto",
            Image = new byte[] { 1, 2 },
            Mail = "a@b.c",
            Hash_mdp = "hash",
            DateInscription = date
        };
        var entity = dto.ToEntities();
        Assert.AreEqual("toto", entity.UserName);
        Assert.AreEqual("a@b.c", entity.Email);
        Assert.AreEqual("hash", entity.PasswordHash);
        Assert.AreEqual(date, entity.DateInscription);
    }

    [TestMethod]
    public void ToEntities_UtilisateurNormalDto_WithId()
    {
        var dto = new UtilisateurNormalDto { Id = "u2", Pseudo = "titi" };
        var entity = dto.ToEntities("u2");
        Assert.AreEqual("u2", entity.Id);
        Assert.AreEqual("titi", entity.UserName);
    }

    [TestMethod]
    public void ToDto_UtilisateurEntities()
    {
        var entity = new UtilisateurEntities
        {
            Id = "u3",
            UserName = "tata",
            Email = "x@y.z",
            PasswordHash = "h2",
            DateInscription = new DateTime(2023, 5, 5)
        };
        var dto = entity.ToDto();
        Assert.AreEqual("u3", dto.Id);
        Assert.AreEqual("tata", dto.Pseudo);
        Assert.AreEqual("x@y.z", dto.Mail);
        Assert.AreEqual("h2", dto.Hash_mdp);
        Assert.AreEqual(new DateTime(2023, 5, 5), dto.DateInscription);
    }

    [TestMethod]
    public void ToEntities_SuccessNormalDto_WithId_2()
    {
        var dto = new SuccessNormalDto { Id = "s3", Nom = "S3", Type = "T3", Image = "img3", Objectif = 3, Description = "desc3", Evenement = "ev3" };
        var entity = dto.ToEntities("s3");
        Assert.AreEqual("s3", entity.Id);
        Assert.AreEqual("S3", entity.Nom);
        Assert.AreEqual("T3", entity.Type);
        Assert.AreEqual("img3", entity.Image);
        Assert.AreEqual(3, entity.Objectif);
        Assert.AreEqual("desc3", entity.Description);
        Assert.AreEqual("ev3", entity.Evenenement);
    }

    [TestMethod]
    public void ToEntities_SuccessStateNormalDto_WithId_2()
    {
        var dto = new SuccessStateNormalDto { Id = "ss3", PercentSucces = 33, IsSucces = true };
        var entity = dto.ToEntities("ss3");
        Assert.AreEqual("ss3", entity.Id);
        Assert.AreEqual(33, entity.PercentSucces);
        Assert.IsTrue(entity.IsSucces);
    }
}
