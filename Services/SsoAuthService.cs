using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using SSO.Client.VAMS.Models;

namespace SSO.Client.VAMS.Services
{
    
    /// Handles SSO API calls
    public class SsoAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SsoAuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// Logs in using SSO API and returns the login response DTO plus any error message returned by the API.
        /// Returns (responseDto, null) on success or (null, errorMessage) on failure.
        public async Task<(LoginResponseDto? Response, string? ErrorMessage)> LoginAsync(string username, string password)
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

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Prefer the body returned by the API as the error message (plain text or JSON string)
                // If the body is empty, use the status code reason.
                var errorMessage = !string.IsNullOrWhiteSpace(content)
                    ? content.Trim('"') // trim quotes if API returned a JSON string
                    : response.ReasonPhrase ?? $"HTTP {(int)response.StatusCode}";

                return (null, errorMessage);
            }

            // Deserialize case-insensitively
            var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return (loginResponse, null);
        }

        /// Gets user claims from /api/auth/me
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
