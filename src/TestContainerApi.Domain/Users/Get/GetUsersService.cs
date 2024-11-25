using TestContainerApi.Domain.Users.Get.ApiServices;

namespace TestContainerApi.Domain.Users.Get
{
    public interface IGetUsersService
    {
        Task<User> GetUsersById(int id);
    }

    public class GetUsersService(IGetUsersRepository getUsersRepository, ICatsApiService catsApiService) : IGetUsersService
    {
        readonly IGetUsersRepository _getUsersRepository = getUsersRepository;
        readonly ICatsApiService _catsApiService = catsApiService;

        public async Task<User> GetUsersById(int id)
        {
            var user = await _getUsersRepository.GetUsersById(id);
            if (user == null)
                return default;

            var catUser = await _catsApiService.GetRandomCat();
            return user;
        }
    }
}
