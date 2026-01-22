using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using SSO.Client.VAMS.Data;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// MVC + Controllers
// ---------------------------
builder.Services.AddControllersWithViews();

// ---------------------------
// HttpClient factory (for SSO API calls)
// ---------------------------
builder.Services.AddHttpClient();

// ---------------------------
// Session middleware
// ---------------------------
builder.Services.AddSession();

// ---------------------------
// EF Core DbContext
// ---------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------------------------
// Authentication: Cookies + OpenID Connect
// ---------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Authority = "https://localhost:5001"; // IdentityServer URL
    options.ClientId = "appA_client";
    options.ResponseType = "code";
    options.UsePkce = true;

    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("sso_api");

    options.TokenValidationParameters.NameClaimType = "name";

    // Allow HTTP for development (since Web App 1 is HTTP)
    options.RequireHttpsMetadata = false;

    // Important: must match RedirectUris in IdentityServerConfig
    options.CallbackPath = "/signin-oidc";
    options.SignedOutCallbackPath = "/signout-callback-oidc";
});

var app = builder.Build();

// ---------------------------
// Middleware pipeline
// ---------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// SESSION must come before Authentication
app.UseSession();

// AUTHENTICATION must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

// ---------------------------
// Default route
// ---------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
