using Microsoft.EntityFrameworkCore;
using SSO.Client.VAMS.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// MVC with controllers and views (Razor Pages projects may use AddRazorPages instead).
builder.Services.AddControllersWithViews();

// HttpClient factory used by SsoAuthService to call external SSO endpoints.
builder.Services.AddHttpClient();

// Session middleware to store simple authenticated state (token, employee id).
builder.Services.AddSession();

// SSO service registration (scoped per-request).
builder.Services.AddScoped<SSO.Client.VAMS.Services.SsoAuthService>();

// Register EF Core DbContext (SQL Server). Uses DefaultConnection from appsettings.json.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
