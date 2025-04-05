using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MoneyManager.Data.Interface;
using MoneyManager.Data.Services;
using MoneyManager.Shared;
using MoneyManager.Shared.UserAuthentication;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

#region Connect Azure AD
var azureAdSection = builder.Configuration.GetSection("AzureAd");
var envClientId = Environment.GetEnvironmentVariable("AzureAd__ClientId");
if (!string.IsNullOrEmpty(envClientId))
{
    azureAdSection["ClientId"] = envClientId;
}

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(azureAdSection);

builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI(); 
#endregion

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMicrosoftIdentityConsentHandler();
builder.Services.AddScoped<IMonthSheet, MonthSheetService>();
builder.Services.AddScoped<ICategory, CategoryService>();
builder.Services.AddScoped<ISeller, SellerService>();
builder.Services.AddScoped<IAccount, AccountService>();
builder.Services.AddScoped<IUserAuthentication, UserAuthenticationService>();

#region Connect with DB
// Load MongoDB settings from configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));

// Register MongoDB client as a singleton service
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    // Check if the environment variable is set (for production)
    var envConn = Environment.GetEnvironmentVariable("MONGO_CLUSTER_CONNECTION_STRING");
    if (!string.IsNullOrEmpty(envConn))
    {
        settings.ConnectionString = envConn;
    }
    return new MongoClient(settings.ConnectionString);
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
