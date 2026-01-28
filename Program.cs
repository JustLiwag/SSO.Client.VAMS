using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "sso_cookie";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("sso_cookie", options =>
{
    options.LoginPath = "/Account/Login";
})
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:5001"; // SSO.Auth.Api
    options.ClientId = "vams_client";
    options.ClientSecret = "vams_secret";

    options.ResponseType = "code";
    options.UsePkce = true;

    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("vams_api");

    options.CallbackPath = "/signin-oidc";
});


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
