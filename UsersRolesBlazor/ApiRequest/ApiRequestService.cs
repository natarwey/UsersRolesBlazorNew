using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using UsersRolesBlazor.ApiRequest.Model;
using UsersRolesBlazor.ApiRequest.Services;

namespace UsersRolesBlazor.ApiRequest
{
    public class ApiRequestService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiRequestService> _logger;

        public ApiRequestService(HttpClient httpClient, ILogger<ApiRequestService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<UserData> GetAllUsersAsync()
        {
            var url = "api/Users/getAllUser";

            try
            {
                var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning("Ответ от сервера пуст.");
                    return new UserData();
                }

                var usersData = JsonSerializer.Deserialize<UserData>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return usersData ?? new UserData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе");
                return new UserData();
            }
        }

        public async Task<UserAddData> AddNewUser(ReqDataUser user)
        {
            var url = "api/Users/createNewUser";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, user);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                var userAddData = JsonSerializer.Deserialize<UserAddData>(responseContent);

                return userAddData ?? new UserAddData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при запросе: {ex.Message}");
                return new UserAddData();
            }
        }

        public async Task<UserUpdateData> UpdateUserAsync(int userId, ReqDataUser user)
        {
            var url = $"api/Users/updateUser/{userId}";

            try
            {
                var response = await _httpClient.PutAsJsonAsync(url, user);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                var userUpdateData = JsonSerializer.Deserialize<UserUpdateData>(responseContent);

                return userUpdateData ?? new UserUpdateData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении пользователя");
                return new UserUpdateData { status = false, message = ex.Message };
            }
        }

        public async Task<UserDeleteData> DeleteUserAsync(int userId)
        {
            var url = $"api/Users/deleteUser/{userId}";

            try
            {
                var response = await _httpClient.DeleteAsync(url);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                var userDeleteData = JsonSerializer.Deserialize<UserDeleteData>(responseContent);

                return userDeleteData ?? new UserDeleteData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении пользователя");
                return new UserDeleteData();
            }
        }

        public async Task<Auth> LoginAsync(string Email, string Password)
        {
            //var url = "http://localhost:7294/api/Users/login";
            var url = $"api/Users/login";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, new { Email, Password });

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var authData = JsonSerializer.Deserialize<AuthData>(responseContent);

                return authData?.user ?? new Auth();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при запросе: {ex.Message}");
                return new Auth();
            }
        }

        public async Task<bool> CheckLoginUniqueAsync(string login)
        {
            //var url = $"http://localhost:5096/api/UsersLogins/checkLoginUnique/{login}";
            var url = $"api/Users/checkLoginUnique/{login}";

            try
            {
                var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonSerializer.Deserialize<CheckLoginUniqueResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.IsUnique ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при проверке уникальности логина");
                return false;
            }
        }
    }
}
