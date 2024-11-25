using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Data;
using TestContainerApi.Domain.Users.Get.ApiServices;

namespace TestContainerApi.WebApi.Test.Endpoints
{
    public class GetUsersEndpointTest(WebTestFactory factory) : IClassFixture<WebTestFactory>
    {
        HttpClient _client = factory.CreateClient();
        IDbConnection _connection = factory._connection;
        Mock<ICatsApiService> _mockCatsApiService;

        [Fact]
        public async Task Get_Users_ReturnsData()
        {
            // Arrange
            var baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "scripts");
            var createTablesPath = Path.Combine(baseDirectory, "insertUser.sql");
            var script = await File.ReadAllTextAsync(createTablesPath);

            using var command = _connection.CreateCommand();
            command.CommandText = script;
            command.ExecuteNonQuery();

            // Act
            var response = await _client.GetAsync("/api/users");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
        }

        [Fact]
        public async Task Get_Users_ReturnsNoContent()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/api/users");

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
        }

        [Fact]
        public async Task Get_UserById_ReturnsData()
        {
            // Arrange
            _mockCatsApiService = new Mock<ICatsApiService>();
            _mockCatsApiService.Setup(service => service.GetRandomCat()).Returns(
                Task.FromResult(new CatResponse { Id = "111", Url = "http://test.com", Height = 0, Width = 0 }));

            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<ICatsApiService>(_mockCatsApiService.Object);
                });
            }).CreateClient();

            var baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "scripts");
            var createTablesPath = Path.Combine(baseDirectory, "insertUser.sql");
            var script = await File.ReadAllTextAsync(createTablesPath);

            using var command = _connection.CreateCommand();
            command.CommandText = script;
            command.ExecuteNonQuery();

            // Act
            var response = await _client.GetAsync("/api/users/1");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
            _mockCatsApiService.Verify(service => service.GetRandomCat(), Times.Once);
        }

        [Fact]
        public async Task Get_UserById_ReturnsNotFound()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/api/users/1");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        }
    }
}
