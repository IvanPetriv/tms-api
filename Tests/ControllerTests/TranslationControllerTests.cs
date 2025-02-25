using Moq;
using Xunit;
using AutoMapper;
using BackendDB.Models;
using APIMain.Controllers;
using Microsoft.EntityFrameworkCore;


namespace APIMain.Tests.ControllerTests;
public class TranslationsControllerTests {
    private readonly Mock<ILogger<ProjectsController>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DbContextOptions<TmsMainContext> _dbContextOptions;

    public TranslationsControllerTests() {
        _loggerMock = new Mock<ILogger<ProjectsController>>();
        _mapperMock = new Mock<IMapper>();

        // Use an in-memory database for testing
        _dbContextOptions = new DbContextOptionsBuilder<TmsMainContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;
    }

    [Fact]
    public void GetById_ReturnsOk_WhenTranslationExists() {

        Assert.Equal(2, int.Parse("3d"));
    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenTranslationDoesNotExist() {
        Assert.Equal(4, 1);
    }
}
