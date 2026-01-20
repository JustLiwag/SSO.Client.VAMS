using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using SSO.Client.VAMS.Models;

namespace SSO.Client.VAMS.Services
{
    /// <summary>
    /// Handles communication with the external SSO API.
    /// This service encapsulates HTTP calls required to authenticate users and fetch their claims.
    /// </summary>
    public class SsoAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Creates a new <see cref="SsoAuthService"/>.
        /// </summary>
        /// <param name="httpClientFactory">Factory to create <see cref="HttpClient"/> instances.</param>
        public SsoAuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Attempts to log the user in using the SSO API.
        /// Returns a tuple: (LoginResponseDto? response, string? errorMessage).
        /// If authentication fails the response is null and errorMessage contains API details when available.
        /// </summary>
        /// <param name="username">Username credential.</param>
        /// <param name="password">Password credential.</param>
        /// <returns>A tuple with the deserialized response DTO on success, or null plus an error message on failure.</returns>
        public async Task<(LoginResponseDto? Response, string? ErrorMessage)> LoginAsync(string username, string password)
        {
            var client = _httpClientFactory.CreateClient();

            var payload = new
            {
                Username = username,
                Password = password
            };

            // Build the request to the SSO API login endpoint.
            var request = new HttpRequestMessage(HttpMethod.Post,
                "https://localhost:5001/api/auth/login")
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Favor the API response body as an error message when present.
                var errorMessage = !string.IsNullOrWhiteSpace(content)
                    ? content.Trim('"') // trim surrounding quotes if API returned a JSON string
                    : response.ReasonPhrase ?? $"HTTP {(int)response.StatusCode}";

                return (null, errorMessage);
            }

            // Deserialize response case-insensitively to map JSON properties to the DTO.
            var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            return (loginResponse, null);
        }

        /// <summary>
        /// Fetches the user's claims from the SSO API using the supplied bearer token.
        /// </summary>
        /// <param name="token">Bearer token obtained during login.</param>
        /// <returns>List of <see cref="UserClaimsDto"/> on success, otherwise null.</returns>
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
