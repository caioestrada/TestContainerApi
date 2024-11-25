using System.Data.SqlClient;

namespace TestContainerApi.WebApi.Test
{
    public class DatabaseTestHelper(string connectionString)
    {
        private readonly string _connectionString = connectionString;

        public void SeedDatabase()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            // Criação de tabelas ou inserção de dados
            var command = connection.CreateCommand();
            command.CommandText = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
            BEGIN
                  CREATE TABLE dbo.Users (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Name NVARCHAR(100) NOT NULL,
                    Email NVARCHAR(100) NOT NULL
                  );

                INSERT INTO Users (Name, Email) VALUES ('Test', 'test1@tes.com'), ('Test2', 'test2@tes.com');
                INSERT INTO Users (Name, Email) VALUES ('Test3', 'test3@tes.com'), ('Test4', 'test4@tes.com');
                INSERT INTO Users (Name, Email) VALUES ('Test5', 'test5@tes.com'), ('Test6', 'test6@tes.com');
            END;";
            command.ExecuteNonQuery();
        }
    }
}
