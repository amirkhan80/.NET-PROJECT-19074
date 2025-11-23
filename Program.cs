using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using MongoDB.Driver;
using SmartServiceHub.Data;
using SmartServiceHub.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ MongoDB
var mongoConn = "mongodb+srv://amirkhan91522_db_user:aDTDIPwA2g0e1WDY@cluster0.mb7ruie.mongodb.net/?retryWrites=true&w=majority";
var mongoDbName = "SmartServiceHubDB";

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConn));
builder.Services.AddSingleton(sp => new MongoDbContext(sp.GetRequiredService<IMongoClient>(), mongoDbName));

// ✅ Services
builder.Services.AddControllersWithViews();
builder.Services.Configure<FormOptions>(o => o.MultipartBodyLengthLimit = 50 * 1024 * 1024);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IServiceTypeService, ServiceTypeService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// ✅ Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

// ✅ Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("MechanicOnly", policy => policy.RequireRole("Mechanic"));
});

// ✅ Build the app (ONLY ONCE)
var app = builder.Build();

// ✅ Seed Default Service Types
using (var scope = app.Services.CreateScope())
{
    var svc = scope.ServiceProvider.GetRequiredService<IServiceTypeService>();
    await SeedServiceTypes.SeedAsync(svc);
}

// ✅ Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ✅ Create wwwroot folders if missing
var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
if (!Directory.Exists(wwwrootPath))
    Directory.CreateDirectory(wwwrootPath);

var vehiclesPath = Path.Combine(wwwrootPath, "vehicles");
if (!Directory.Exists(vehiclesPath))
    Directory.CreateDirectory(vehiclesPath);

// ✅ Default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
