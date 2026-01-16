namespace SSO.Client.VAMS.Models
{
    /// Represents the response returned by SSO API after login
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string EmployeeNo { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
    }
}
