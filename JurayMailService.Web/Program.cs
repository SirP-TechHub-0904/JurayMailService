using Domain.Models;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Extensions;
using PostmarkEmailService;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using JurayMailService.Web.Background;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AppConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddApplicationCustomServices();

//

builder.Services.AddTransient<PostmarkClient>(_ => new PostmarkClient(builder.Configuration.GetSection("PostmarkSettings")["ServerToken"]));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
  
builder.Services.AddRazorPages();
//builder.Services.AddHostedService<BackgroundServiceJob>();
builder.Services.AddHostedService<TimerService>();
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false; // <-- disable special characters
    options.Password.RequireLowercase = false;
})
            .AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";           // Redirect when not logged in
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Redirect when access denied
    options.ExpireTimeSpan = TimeSpan.FromMinutes(300);
    options.SlidingExpiration = true;
});
//builder.Services.AddSingleton<BackgroundServiceJob>();
//builder.Services.AddHostedService<BackgroundServiceJob>(provider => provider.GetService<BackgroundServiceJob>());



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
