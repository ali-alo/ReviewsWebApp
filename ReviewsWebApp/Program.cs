using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Data;
using ReviewsWebApp.Options;
using ReviewsWebApp.Repositories;
using ReviewsWebApp.Repositories.Interfaces;
using ReviewsWebApp.Services;
using ReviewsWebApp.Services.Interfaces;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var config = builder.Configuration;
var connectionString = config.GetConnectionString("DefaultConnection");

services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
services.AddDatabaseDeveloperPageExceptionFilter();
services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
services.AddLocalization(options => options.ResourcesPath = "Resources");
services.AddControllersWithViews().AddViewLocalization();

services.Configure<RequestLocalizationOptions>(options => 
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("ru"),
    };
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedUICultures = supportedCultures;
    options.SupportedCultures = supportedCultures;

});
services.Configure<AzureOptions>(config.GetSection("Azure"));

services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
services.AddTransient<IImageService, ImageService>();
services.AddTransient<ITagService, TagService>();
services.AddScoped<IReviewItemRepository, ReviewItemRepository>();
services.AddScoped<IReviewRepository, ReviewRepository>();
services.AddScoped<IReviewGroupRepository, ReviewGroupRepository>();
services.AddScoped<ICommentRepository, CommentRepository>();
services.AddScoped<IUserRatedReviewRepository, UserRatedReviewRepository>();
services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = config["Authentication:Google:ClientId"];
        options.ClientSecret = config["Authentication:Google:ClientSecret"];
    })
    .AddTwitter(options =>
    {
        options.ConsumerKey = config["Authentication:Twitter:ConsumerAPIKey"];
        options.ConsumerSecret = config["Authentication:Twitter:ConsumerSecret"];
        options.AccessDeniedPath = "/";
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
