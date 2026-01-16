using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using SSO.Client.VAMS.Models;

namespace SSO.Client.VAMS.Services
{
    /// <summary>
    /// Handles SSO API calls
    /// </summary>
    public class SsoAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SsoAuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Logs in using SSO API and returns the login response DTO
        /// </summary>
        public async Task<LoginResponseDto?> LoginAsync(string username, string password)
        {
            var client = _httpClientFactory.CreateClient();

            var payload = new
            {
                Username = username,
                Password = password
            };

            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://localhost:5001/api/auth/login")
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            // Deserialize case-insensitively
            var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return loginResponse;
        }

        /// <summary>
        /// Gets user claims from /api/auth/me
        /// </summary>
        public async Task<List<UserClaimsDto>?> GetUserClaimsAsync(string token)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:5001/api/auth/me");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            var claims = JsonSerializer.Deserialize<List<UserClaimsDto>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return claims;
        }
    }
}
