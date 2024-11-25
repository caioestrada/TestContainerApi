using System.Net.Http.Json;

namespace TestContainerApi.Domain.Users.Get.ApiServices
{
    public interface ICatsApiService
    {
        Task<CatResponse> GetRandomCat();
    }

    public class CatsApiService(HttpClient httpClient) : ICatsApiService
    {
        readonly HttpClient _httpClient = httpClient;

        public async Task<CatResponse> GetRandomCat()
        {
            return (await _httpClient.GetFromJsonAsync<IEnumerable<CatResponse>>("images/search")).FirstOrDefault();
        }
    }
}
