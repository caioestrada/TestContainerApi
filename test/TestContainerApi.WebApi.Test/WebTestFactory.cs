using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;
using Testcontainers.MsSql;

namespace TestContainerApi.WebApi.Test
{
    public class WebTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _sqlContainer;
        public IDbConnection _connection { get; private set; }

        public WebTestFactory()
        {
            _sqlContainer = new MsSqlBuilder()
            .WithPassword("YourStrongPassword123!")
            .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IDbConnection>(_ => _connection);
                //services.AddSingleton(new DatabaseTestHelper("Server=localhost,1433;Database=master;User Id=sa;Password=YourStrongPassword123!;"));
            });

            builder.UseEnvironment("Development");
        }

        public async Task InitializeAsync()
        {
            await _sqlContainer.StartAsync();

            _connection = new SqlConnection(_sqlContainer.GetConnectionString());
            _connection.Open();

            var baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "scripts");
            var createTablesPath = Path.Combine(baseDirectory, "create.sql");
            var script = await File.ReadAllTextAsync(createTablesPath);

            using var command = _connection.CreateCommand();
            command.CommandText = script;
            command.ExecuteNonQuery();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            if (_connection?.State == ConnectionState.Open)
                _connection.Close();

            await _sqlContainer.DisposeAsync();
        }
    }
}
