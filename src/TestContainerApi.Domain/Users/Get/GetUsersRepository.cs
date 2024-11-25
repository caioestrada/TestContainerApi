using Dapper;
using System.Data;

namespace TestContainerApi.Domain.Users.Get
{
    public interface IGetUsersRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUsersById(int id);
    }

    public class GetUsersRepository(IDbConnection dbConnection) : IGetUsersRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;

        public async Task<IEnumerable<User>> GetUsers()
        {
            const string sql = "SELECT Id, Name, Email FROM Users";
            return await _dbConnection.QueryAsync<User>(sql);
        }

        public async Task<User> GetUsersById(int id)
        {
            const string sql = "SELECT Id, Name, Email FROM Users WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }
    }
}
