using Microsoft.AspNetCore.Authentication.Cookies;
using Shared.Application.Configuration;
using Shared.Application.Localization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
IConfigurationSection homeConfig = configuration.GetSection("HomeConfig");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.Configure<TssConfig>(configuration.GetSection("TssConfig"));
builder.Services.Configure<EsmConfig>(configuration.GetSection("EsmConfig"));
builder.Services.Configure<IdentityConfig>(configuration.GetSection("IdentityConfig"));
builder.Services.AddUtconnectLocalization();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            context.HttpContext.Response.Redirect(homeConfig["Url"]!);
            return Task.CompletedTask;
        };
    });

WebApplication app = builder.Build();

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();