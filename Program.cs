using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//REQUIRED FOR OIDC COOKIE SIGN-IN
builder.Services.AddHttpContextAccessor();

//Debug purposes
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Trace);


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "vams_cookie";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("vams_cookie", options =>
{
    options.Cookie.Name = "VAMS.Auth";
    options.Cookie.Path = "/";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
})
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:5001";
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

    options.MapInboundClaims = false;

    options.Events = new OpenIdConnectEvents
    {
        OnTokenValidated = context =>
        {
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            context.Response.Redirect("/Home/Error?msg=" + context.Exception.Message);
            context.HandleResponse();
            return Task.CompletedTask;
        }
    };
    options.Events = new OpenIdConnectEvents
    {
        OnMessageReceived = context =>
        {
            Console.WriteLine("OIDC: Message received");
            return Task.CompletedTask;
        },
        OnAuthorizationCodeReceived = context =>
        {
            Console.WriteLine("OIDC: Authorization code received");
            return Task.CompletedTask;
        },
        OnTokenResponseReceived = context =>
        {
            Console.WriteLine("OIDC: Token response received");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("OIDC: Token validated");
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("OIDC ERROR: " + context.Exception);
            return Task.CompletedTask;
        }
    };

});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCookiePolicy();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
